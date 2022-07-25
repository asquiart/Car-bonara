import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { PaymentMethod, User, UserState } from 'src/app/model/user';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { UserManagementService } from 'src/app/services/user-management.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'cb-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  constructor(
    private authenticationService: AuthenticationService,
    private utitityService: UtilityService,
    public usermanagmentService: UserManagementService
  ) {

  }
  availablePaymentMethods = [
    { name: "Paypal", imageUrl: "./assets/images/payments/paypal.png", method: PaymentMethod.Paypal },
    { name: "SEPA Lastschrift", imageUrl: "./assets/images/payments/sepa.png", method: PaymentMethod.Sepa },
    { name: "Visa", imageUrl: "./assets/images/payments/visa.png", method: PaymentMethod.Visa },
    { name: "Mastercard", imageUrl: "./assets/images/payments/mastercard.png", method: PaymentMethod.Mastercard },
    { name: "Maestro", imageUrl: "./assets/images/payments/maestro.png", method: PaymentMethod.Maestro },
    { name: "GiroPay", imageUrl: "./assets/images/payments/giropay.png", method: PaymentMethod.Giropay },
  ];

  ngOnInit(): void {
    this.usermanagmentService.loadUsers();



  }

  getDate(number: number) {
    let date = this.utitityService.UTCtoLocal(new Date(number));
    return this.utitityService.getDateTimeString(date);
  }

  lockUser(user: User) {
    user.state = UserState.Locked;
    this.usermanagmentService.setUserStauts(user).subscribe();
  }
  authorizeUser(user: User) {
    user.state = UserState.Authorized;
    this.usermanagmentService.setUserStauts(user).subscribe();
  }
  unauthorizeUser(user: User) {
    user.state = UserState.Unauthorized;
    this.usermanagmentService.setUserStauts(user).subscribe();
  }

  impersonate(user: User) {
    this.authenticationService.impersonate(user);
  }

}
