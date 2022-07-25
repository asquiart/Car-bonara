import { HttpClient } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { Observable, Subscription, timer } from 'rxjs';
import { Login } from '../model/login';
import { LoginRequest } from '../model/login-request';
import { Person } from '../model/person';
import { User } from '../model/user';
import jwtDecode from 'jwt-decode';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Routing } from '../app-routing.module';
import { state } from '@angular/animations';
import { AppComponent } from '../app.component';
import { NavigationComponent } from '../components/navigation/navigation.component';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService implements CanActivate {

  static STORAGE_KEY = "login";
  static STORAGE_EMPL_KEY = "login_employee";
  static loginUrl: string = "/api/auth/login";
  static impersonateUrl: string = "/api/auth/impersonate/";
  static changeUrl: string = "/api/auth/change";

  private loginData: Login | null = null;
  private token: CarbonaraToken | null = null;
  private tokenExpiredTimerSubscription: Subscription | null = null;

  loggedInEvent = new EventEmitter<Person>();
  loggedOutEvent = new EventEmitter();

  constructor(
    private http: HttpClient,
    private snackbarService: MatSnackBar,
    private router: Router
  ) 
  {
    //Load previously saved logins, if not already logged in
    this.loadLogin();
  }

  public isLoggedIn()
  {
    return this.loginData != null;
  }

  public getPerson() : Person | null
  {
    return this.loginData?.person ?? null;
  }

  public getToken() : string {
    return this.loginData?.token ?? "";
  }

  public login(login: LoginRequest) : Promise<Login> {
    return new Promise((res, rej) => {
      this.http.post<Login>(AuthenticationService.loginUrl, login).subscribe({
        next: (logindata) => {
          this.internalLogin(logindata);
          res(logindata);
        },
        error: (e) => {
          rej(e);
        }
      });
    });
  }

  public isEmployee() : boolean
  {
    return this.hasRole(CarbonaraRole.EMPLOYEE);
  }

  public isAdmin() : boolean {
    return this.hasRole(CarbonaraRole.ADMIN);
  }

  public hasRole(role: CarbonaraRole)
  {
    return (this.isLoggedIn() && this.token?.roles.includes(role)) ?? false;
  }

  public loginRegister(login: Login)
  {
    if (!this.isLoggedIn())
    {
      this.internalLogin(login);
    }
  }

  private saveLogin()
  {
    if (this.loginData)
    {
      localStorage.setItem(AuthenticationService.STORAGE_KEY, JSON.stringify(this.loginData));
    }
  }

  private loadLogin() {
    if (!this.isLoggedIn()) {
      let loaded = localStorage.getItem(AuthenticationService.STORAGE_KEY);
      if (loaded) {
        this.internalLogin(JSON.parse(loaded))
      }
    }
  }




  //Internally do everything after being logged in
  private internalLogin(login: Login)
  {
    this.loginData = login;
    this.internalDecodeToken(login);
    this.saveLogin();
    this.loggedInEvent.emit(login.person);
  }

  private internalDecodeToken(login: Login)
  {
    this.token = CarbonaraToken.fromToken(login.token);
    let timeUntilExpired = this.token.exp*1000 - new Date().getTime() - 1000;
    this.tokenExpiredTimerSubscription = timer(timeUntilExpired).subscribe(() => {
      this.logout();
      this.snackbarService.open("Du wurdest automatisch abgemeldet");
    });
  }

  public logout()
  {
    //Delete the localStorage-entries
    this.stopImpersonating();
    localStorage.removeItem(AuthenticationService.STORAGE_KEY);
    //Clear session data
    this.loginData = null;
    this.token = null;
    if (this.tokenExpiredTimerSubscription)
    {
      this.tokenExpiredTimerSubscription.unsubscribe();
      this.tokenExpiredTimerSubscription = null;
    }
    this.loggedOutEvent.emit();
  }

  public changePassword(oldPassword : string , newPassword : string) : Observable<any>
  {
    return this.http.put(AuthenticationService.changeUrl, [oldPassword , newPassword] );
  }


  public impersonate(user: User)
  {
    if (this.isLoggedIn() && this.isEmployee() && !this.isImpersonating())
    {
      this.http.get<Login>(AuthenticationService.impersonateUrl + user.id).subscribe(login => {
        if (login)
        {
          localStorage.setItem(AuthenticationService.STORAGE_EMPL_KEY, JSON.stringify(this.loginData));
          this.internalLogin(login as Login);
        }
      });
    }
  }

  public stopImpersonating() 
  {
    if (this.isLoggedIn() && this.isImpersonating())
    {
      let employeeToken = localStorage.getItem(AuthenticationService.STORAGE_EMPL_KEY)!;
      localStorage.removeItem(AuthenticationService.STORAGE_EMPL_KEY);
      let employeeLogin = JSON.parse(employeeToken) as Login;
      this.internalLogin(employeeLogin);
    }    
  }

  public isImpersonating() : boolean
  {
    return localStorage.getItem(AuthenticationService.STORAGE_EMPL_KEY) != null;
  }

  
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    const guestOnly = [
      Routing.ROUTE_ROOT,
      Routing.ROUTE_WELCOME,
      Routing.ROUTE_LOGIN,
      Routing.ROUTE_REGISTER,
    ]
    const guestAllowed = [
      Routing.ROUTE_PLANS,
      Routing.ROUTE_BOOKING_CREATE      
    ]

    let path = "/" + route.routeConfig?.path;
    if (!path)
      return false;

    if (path.startsWith(Routing.ROUTE_ADMIN))
      return this.checkCanActivate(this.isAdmin());

    if (path.startsWith(Routing.ROUTE_EMPLOYEE))
      return this.checkCanActivate(this.isEmployee());

    if (guestOnly.includes(path))
      return this.checkCanActivate(!this.isLoggedIn());

    if (guestAllowed.includes(path))
      return true;

    return this.checkCanActivate(this.isLoggedIn());
  }


  private checkCanActivate(canActivate: boolean): boolean {
    
    if (!canActivate && !this.router.getCurrentNavigation()?.previousNavigation)
    {
      this.router.navigate([NavigationComponent.getStartpage(this)]);
      return false;
    }
    return canActivate;
  }
}



class CarbonaraToken {
  private constructor() {
  }

  public Actor!: number;
  public User!: number;
  public aud!: string;
  public exp!: number;
  public roles: string[] = [];

  public static fromToken(jwtToken: string) : CarbonaraToken {
    let token = new CarbonaraToken();
    const ROLE_ATTRIBUTE = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    const tokenContent = jwtDecode(jwtToken) as any;

    if (ROLE_ATTRIBUTE in tokenContent)
    {
      token.roles = tokenContent[ROLE_ATTRIBUTE] as string[];
    }
    Object.assign(token, tokenContent);
    return token;
  }

}

export enum CarbonaraRole {
  AUTHORIZED_USER = "Authorized",
  EMPLOYEE = "Employee",
  ADMIN = "Admin"
}