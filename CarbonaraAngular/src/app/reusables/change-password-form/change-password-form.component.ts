import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { carbonaraPasswordValidator, checkPassword } from 'src/app/forms/password-validator';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-change-password-form',
  templateUrl: './change-password-form.component.html',
  styleUrls: ['./change-password-form.component.scss']
})
export class ChangePasswordFormComponent implements OnInit {

  form!: FormGroup;

  constructor(
    private dialogRef: MatDialogRef<ChangePasswordFormComponent>,
    private authService: AuthenticationService,
    private snackbarService: MatSnackBar,
    formBuilder: FormBuilder,
  ) { 

    this.form = formBuilder.group({
      oldPassword: ["", [Validators.required]],
      newPassword: ["", [Validators.required ,carbonaraPasswordValidator()]]  
    });

  }

  
  ngOnInit(): void {
  }




  cancel()
  {
    this.dialogRef.close(null);
  }
  submit()
  {
    this.authService.changePassword(this.form.value.oldPassword ,this.form.value.newPassword ).subscribe({
      next: ()=>{
        this.snackbarService.open("Passwort erfolgreich geändert");
      },
      error: () => {
        this.snackbarService.open("Passwort konnte nicht geändert werden");
      }
    }
      
    );
    this.dialogRef.close(null);
  }


  getPasswordErrors() : string {
    let passwordControl = this.form.get('newPassword');
    let passwordErrors = checkPassword(passwordControl?.value as string);
    if (!passwordErrors)
      return "";
    else return passwordErrors.errorString;
  }


}
