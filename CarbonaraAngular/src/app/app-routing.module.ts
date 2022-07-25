import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookingComponent } from './components/booking/booking.component';
import { HistoryComponent } from './components/history/history.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { OverviewComponent } from './components/overview/overview.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { StartComponent } from './components/employee/start/start.component';
import { ChangeUserdateComponent } from './components/change-userdate/change-userdate.component';
import { CarListComponent } from './components/employee/car-list/car-list.component';
import { CarclassListComponent } from './components/employee/carclass-list/carclass-list.component';
import { CartypeListComponent } from './components/employee/cartype-list/cartype-list.component';
import { PlanListComponent } from './components/employee/plan-list/plan-list.component';
import { StationListComponent } from './components/employee/station-list/station-list.component';
import { UserListComponent } from './components/employee/user-list/user-list.component';
import { EmployeeListComponent } from './components/employee/employee-list/employee-list.component';
import { PlanOverviewComponent } from './components/plan-overview/plan-overview.component';
import { AuthenticationService } from './services/authentication.service';

export class Routing {
  private static PATH_ROOT = "";
  private static PATH_WELCOME = "welcome";
  private static PATH_OVERVIEW = "overview";
  private static PATH_LOGIN = "login";
  private static PATH_REGISTER = "register";
  private static PATH_BOOKING_CREATE = "booking/create";
  private static PATH_BOOKING_HISTORY = "booking/history";
  private static PATH_USER = "user";
  private static PATH_USER_EDIT = Routing.PATH_USER + "/edit";
  private static PATH_PLANS = "plans";
  //EMPLOYEE
  private static PATH_EMPLOYEE = "employee";
  private static PATH_EMPLOYEE_CARLIST = this.PATH_EMPLOYEE + "/cars";
  private static PATH_EMPLOYEE_CARCLASSES = this.PATH_EMPLOYEE + "/carclasses";
  private static PATH_EMPLOYEE_CARTYPES = this.PATH_EMPLOYEE + "/cartypes";
  private static PATH_EMPLOYEE_PLANS = this.PATH_EMPLOYEE + "/plans";
  private static PATH_EMPLOYEE_STATIONS = this.PATH_EMPLOYEE + "/stations";
  private static PATH_EMPLOYEE_USERS = this.PATH_EMPLOYEE + "/users";
  //ADMIN 
  private static PATH_ADMIN = "admin";
  private static PATH_ADMIN_EMPLOYEES = this.PATH_ADMIN + "/employees";

  static ROUTE_ROOT = "/" + this.PATH_ROOT;
  static ROUTE_WELCOME = "/" + this.PATH_WELCOME;
  static ROUTE_OVERVIEW = "/" + this.PATH_OVERVIEW;
  static ROUTE_LOGIN = "/" + this.PATH_LOGIN;
  static ROUTE_REGISTER = "/" + this.PATH_REGISTER;
  static ROUTE_BOOKING_CREATE = "/" + this.PATH_BOOKING_CREATE;
  static ROUTE_BOOKING_HISTORY = "/" + this.PATH_BOOKING_HISTORY;
  static ROUTE_USER = "/" + this.PATH_USER;
  static ROUTE_USER_EDIT = "/" + this.PATH_USER_EDIT;
  static ROUTE_PLANS = "/" + this.PATH_PLANS;
  //EMPLOYEE
  static ROUTE_EMPLOYEE = "/" + Routing.PATH_EMPLOYEE;
  static ROUTE_EMPLOYEE_CARLIST = "/" + Routing.PATH_EMPLOYEE_CARLIST;
  static ROUTE_EMPLOYEE_CARCLASSES = "/" + Routing.PATH_EMPLOYEE_CARCLASSES;
  static ROUTE_EMPLOYEE_CARTYPES = "/" + Routing.PATH_EMPLOYEE_CARTYPES;
  static ROUTE_EMPLOYEE_PLANS = "/" + Routing.PATH_EMPLOYEE_PLANS;
  static ROUTE_EMPLOYEE_STATIONS = "/" + Routing.PATH_EMPLOYEE_STATIONS;
  static ROUTE_EMPLOYEE_USERS = "/" + Routing.PATH_EMPLOYEE_USERS;
  
  //ADMIN 
  static ROUTE_ADMIN = "/" + Routing.PATH_ADMIN;
  static ROUTE_ADMIN_EMPLOYEES = "/" + Routing.PATH_ADMIN_EMPLOYEES;
  

  static routes: Routes = [
    //Employee
    { canActivate: [AuthenticationService], path: Routing.PATH_EMPLOYEE, component: StartComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_EMPLOYEE_CARLIST, component: CarListComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_EMPLOYEE_CARCLASSES, component: CarclassListComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_EMPLOYEE_CARTYPES, component: CartypeListComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_EMPLOYEE_PLANS, component: PlanListComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_EMPLOYEE_STATIONS, component: StationListComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_EMPLOYEE_USERS, component: UserListComponent },
    //canActivate: [AuthenticationService], Admin
    { canActivate: [AuthenticationService], path: Routing.PATH_ADMIN_EMPLOYEES, component: EmployeeListComponent },
    //canActivate: [AuthenticationService], Users/all
    { canActivate: [AuthenticationService], path: Routing.PATH_USER_EDIT, component: ChangeUserdateComponent },
    { path: Routing.PATH_USER, pathMatch: 'full', redirectTo: Routing.ROUTE_USER_EDIT },
    { canActivate: [AuthenticationService], path: Routing.PATH_BOOKING_HISTORY, component: HistoryComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_BOOKING_CREATE, component: BookingComponent },
    { path: "booking", redirectTo: '/booking/create', pathMatch: 'full' },
    { canActivate: [AuthenticationService], path: Routing.PATH_REGISTER, component: RegisterComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_LOGIN, component: LoginComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_OVERVIEW, component: OverviewComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_WELCOME, component: WelcomeComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_ROOT, component: WelcomeComponent, pathMatch: "full" },
    { canActivate: [AuthenticationService], path: Routing.PATH_WELCOME, component: WelcomeComponent },
    { canActivate: [AuthenticationService], path: Routing.PATH_PLANS, component: PlanOverviewComponent },
    { path: "**", redirectTo: Routing.ROUTE_WELCOME },
  ];
}

@NgModule({
  imports: [RouterModule.forRoot(Routing.routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
