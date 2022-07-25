import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { User } from '../model/user';
import { Login } from '../model/login';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(
    private authenticationService: AuthenticationService,
    private http: HttpClient,
  ) {
    this.authenticationService.loggedInEvent.subscribe(user => {
      this.initialize();
    });
    this.initialize();
  }

  private initialize() {
  }

  //URL
  static userURL  = "/api/user";

  
  public registerUser(registrationData: RegistrationComposite)  : Login | null{
    if (this.authenticationService.isLoggedIn())
      return null;
    this.http.put<Login>(UserService.userURL + "/register" , registrationData).subscribe({
      next: (result) => {
        this.authenticationService.loginRegister(result);
        return result ;
      },
      error: (error) => {
        console.error(error);
        return null;
      }

    });
    return null;
  }

  public currentUserData() : Observable<User>
  {
    return this.http.get<User>(UserService.userURL + "/current");
  }
  public updateUser(user : User) : Observable<any>
  {
    return this.http.put(UserService.userURL + "/update" , user);
  }
  public getNewCardId(user : User) : Observable<Number>
  {
    return this.http.put<Number>(UserService.userURL + "/getnewcard" , null);
  }
  
}

export class RegistrationComposite 
{
  constructor(
    public user: User,
    public password: string
  ) {}
}