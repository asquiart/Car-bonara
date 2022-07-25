import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Person } from 'src/app/model/person';
import { User } from 'src/app/model/user';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'reuse-userdisplay',
  templateUrl: './user-display.component.html',
  styleUrls: ['./user-display.component.scss']
})
export class UserDisplayComponent implements OnInit {

  constructor(
    public authenticationService: AuthenticationService,
    private router: Router
  ) { }

  @Output() click = new EventEmitter<any>();

  guestName : string = "Gast";

  ngOnInit(): void {
  }

  getDisplayName() : string {
    if (this.authenticationService.isLoggedIn())
    {
      let user = this.authenticationService.getPerson() as Person;
      return user.firstname + " " + user.lastname;
    } else
    {
      return this.guestName;
    }
  }

  onChipClick()
  {
    this.click.emit();
  }

}
