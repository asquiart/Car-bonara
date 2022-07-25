import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Routing } from 'src/app/app-routing.module';
import { Login } from 'src/app/model/login';
import { LoginRequest } from 'src/app/model/login-request';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;

  forgotPasswordAddress = "mailto://forgot-password@car-bonara.de?subject=Passwort%20vergessen";
  
  constructor(
    formBuilder: FormBuilder,
    private authenticationService: AuthenticationService,
    private router: Router,
    private snackbarService: MatSnackBar
    ){

      this.loginForm = formBuilder.group({
        password: [ '', Validators.required ],
        email: [ '', [Validators.required, Validators.email] ]
      });
  }

  ngOnInit(): void {
  }

  login() {
    let login = new LoginRequest(this.loginForm.value['email'], this.loginForm.value['password']);
    this.authenticationService.login(login).then(() => {}, () => {
      //Rejected
      this.snackbarService.open("Deine Email oder dein Passwort ist falsch");
    });
  }

}

