import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { User } from '../model/user';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class UserManagementService {

  constructor(
    private authenticationService: AuthenticationService,
    private http: HttpClient,
  ) {
    this.authenticationService.loggedInEvent.subscribe(user => {
      this.initialize();
    });
    this.initialize();
    authenticationService.loggedOutEvent.subscribe(() => this.onLogout());
  }

  private onLogout(){
    this.user = [];
  }

  private initialize() {
  }

  //URLs
  static usermanagmentURL  = "/api/userManagement";

  //Loaded Data
  user : User[] = [];

  public loadUsers()
  {
    this.http.get<User[]>(UserManagementService.usermanagmentURL + "/getuserlist").subscribe(user => this.user = user);
  }
 
  public setUserStauts(userWithNewStatus : User) : Observable<any>
  {
    return this.http.patch(UserManagementService.usermanagmentURL + "/setuserstatus" , userWithNewStatus);
  }
}
