import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { interval, Subscription } from 'rxjs';
import { Routing } from './app-routing.module';
import { AuthenticationService } from './services/authentication.service';
import { DatabaseService } from './services/database.service';
import { NetworkService } from './services/network.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(
    private databaseService: DatabaseService,
    public networkService: NetworkService
  ) {

    

  }

}
