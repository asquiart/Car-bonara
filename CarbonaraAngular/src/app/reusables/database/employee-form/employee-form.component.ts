import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Employee } from 'src/app/model/Employee';
import { Person } from 'src/app/model/person';
import { DatabaseService } from 'src/app/services/database.service';

@Component({
  selector: 'cb-employee-form',
  templateUrl: './employee-form.component.html',
  styleUrls: ['./employee-form.component.scss']
})
export class EmployeeFormComponent implements OnInit {

  form!: FormGroup;

 
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Employee,
    private dialogRef: MatDialogRef<EmployeeFormComponent>,
    formBuilder: FormBuilder,
   public databaseService : DatabaseService

  ) { 

    let defaultData : Employee = this.data ?? Employee.prototype;
    if(this.data == null) 
        defaultData.person = Person.prototype;

    this.form = formBuilder.group({
      firstname: [defaultData.person.firstname, [Validators.required, Validators.minLength(1)]],
      lastname: [defaultData.person.lastname, [Validators.required, Validators.minLength(1)]],
      formOfAddress: [defaultData.person.formOfAddress],
      title: [defaultData.person.title],
      email: [defaultData.person.email, [Validators.required, Validators.email]],
      isAdmin: [defaultData.isAdmin],
      
    });
  }

  working: boolean = false;

  ngOnInit(): void {
  }

  onSubmit() {
    let value : Employee = Employee.prototype;
    Object.assign(value, this.form.value);
    this.dialogRef.close(value);
  }

  cancel()
  {
    this.dialogRef.close(null);
  }
}
