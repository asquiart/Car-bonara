import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Employee } from '../model/Employee';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

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

  private initialize() {
  }



  onLogout()
  {
    this.employees = [];
  }

  //URLs
  static adminURL  = "/api/admin";

  //Loaded Data
  employees : Employee[] = [];

  public loadEmployees()
  {
    this.http.get<Employee[]>(AdminService.adminURL + "/getall").subscribe(employees => {this.employees = employees;});
  }
  public getEmployee(id : number) : Observable<Employee>
  {
    return this.http.get<Employee>(AdminService.adminURL + "/get/" + id  );
  }
  public updateEmployee(id : number ,newData : Employee) : Observable<any>
  {
    return this.http.put(AdminService.adminURL + "/change/" + id , newData);
  }
  public deleteEmployee(id : number ) : Observable<any>
  {
    return this.http.delete(AdminService.adminURL + "/delete/" + id );
  }
  public addEmployee(newData : Employee) : Observable<any>
  {
    return this.http.post(AdminService.adminURL + "/add" , newData);
  }
  
}
