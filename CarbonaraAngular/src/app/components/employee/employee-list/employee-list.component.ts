import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Component, OnInit } from '@angular/core';
import { Employee } from 'src/app/model/Employee';
import { AdminService } from 'src/app/services/admin.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UtilityService } from 'src/app/services/utility.service';
import { EmployeeFormComponent } from 'src/app/reusables/database/employee-form/employee-form.component';
import { Person } from 'src/app/model/person';

@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.scss']
})
export class EmployeeListComponent implements OnInit {

  constructor(
    private authenticationService: AuthenticationService,
    private utitityService: UtilityService,
    public adminService: AdminService, 
    private dialogService: MatDialog,
    private snackbarService: MatSnackBar
  ) {
   
  }

 
  ngOnInit(): void {
      this.adminService.loadEmployees();
  }

  getDate(number : number)
  {
    let date = this.utitityService.UTCtoLocal( new Date( number));
    return this.utitityService.getDateTimeString(date );
  }
  addNew()
  {
    const dialog = this.dialogService.open(EmployeeFormComponent, { data: null });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        let newOne = Employee.prototype;
        newOne.person = Person.prototype;
        newOne.isAdmin = result.isAdmin;
        if(!newOne.isAdmin)
          newOne.isAdmin = false;
        newOne.person.firstname = result.firstname;
        newOne.person.lastname = result.lastname;
        newOne.person.formOfAddress = result.formOfAddress;
        newOne.person.title = result.title;
        newOne.person.email = result.email;

        this.adminService.addEmployee(newOne).subscribe({
          next: () => {
            this.adminService.loadEmployees();
          },
          error: () => {
            this.snackbarService.open("Angestellter konnte nicht hinzugefügt werden");
          }
        });


      }
    });
  }

  change(employee :Employee)
  {
    const dialog = this.dialogService.open(EmployeeFormComponent, { data: employee });

    


    dialog.afterClosed().subscribe(result => {
      if (result)
      {

        let newOne = employee;
        newOne.isAdmin = result.isAdmin;
        newOne.person.firstname = result.firstname;
        newOne.person.lastname = result.lastname;
        newOne.person.formOfAddress = result.formOfAddress;
        newOne.person.title = result.title;
        newOne.person.email = result.email;

        this.adminService.updateEmployee(employee.person.id, newOne).subscribe({
          next: () => {
            this.adminService.loadEmployees();
          },
          error: () => {
            this.snackbarService.open("Angestellter konnte nicht geändert werden");
          }
        });
      }
    });
  }

  deleteEmployee(employee : Employee)
  {
    console.warn(employee);
     this.adminService.deleteEmployee(employee.person.id).subscribe({
      next: ()=>{
        this.adminService.loadEmployees();
      },
    });


  }

}
