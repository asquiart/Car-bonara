import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavigationComponent } from './components/navigation/navigation.component';
import { LayoutModule } from '@angular/cdk/layout';

//MaterialDesign
import { MatCommonModule, MatNativeDateModule } from '@angular/material/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatDividerModule } from '@angular/material/divider';
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule, MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input'
import { MatGridListModule } from '@angular/material/grid-list'
import { MatAutocompleteModule } from '@angular/material/autocomplete'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatCardModule } from '@angular/material/card'
import { MatSelectModule } from '@angular/material/select'
import { MatExpansionModule } from '@angular/material/expansion';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatSnackBarModule, MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';

import { UserDisplayComponent } from './reusables/user-display/user-display.component';
import { AuthenticationService } from './services/authentication.service';
import { StartComponent } from './components/employee/start/start.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { STEPPER_GLOBAL_OPTIONS } from '@angular/cdk/stepper';
import { PaymentSelectComponent } from './reusables/payment-select/payment-select.component';
import { BookingComponent } from './components/booking/booking.component';
import { HistoryComponent } from './components/history/history.component';
import { BookingDetailPanelComponent } from './reusables/booking-detail-panel/booking-detail-panel.component';
import { CarbonaraIconComponent } from './reusables/carbonara-icon/carbonara-icon.component';
import { UtilityService } from './services/utility.service';
import { BookingService } from './services/booking.service';
import { UserService } from './services/user.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { StationSelectComponent } from './reusables/station-select/station-select.component';
import { DatabaseService } from './services/database.service';
import { OverviewComponent } from './components/overview/overview.component';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { TokenInterceptor } from './services/http-interceptor';
import { PlanSelectComponent } from './reusables/plan-select/plan-select.component';
import { AdminService } from './services/admin.service';
import { CarService } from './services/car.service';
import { PlanFormComponent } from './reusables/database/plan-form/plan-form.component';
import { CarFormComponent } from './reusables/database/car-form/car-form.component';
import { AddressInputComponent } from './reusables/address-input/address-input.component';
import { CarclassFormComponent } from './reusables/database/carclass-form/carclass-form.component';
import { CartypeFormComponent } from './reusables/database/cartype-form/cartype-form.component';
import { StationFormComponent } from './reusables/database/station-form/station-form.component';
import { EmployeeFormComponent } from './reusables/database/employee-form/employee-form.component';
import { UserdataChangeFormComponent } from './reusables/userdata-change-form/userdata-change-form.component';
import { ChangeUserdateComponent } from './components/change-userdate/change-userdate.component';
import { UserListComponent } from './components/employee/user-list/user-list.component';
import { CarListComponent } from './components/employee/car-list/car-list.component';
import { CartypeListComponent } from './components/employee/cartype-list/cartype-list.component';
import { PlanListComponent } from './components/employee/plan-list/plan-list.component';
import { CarclassListComponent } from './components/employee/carclass-list/carclass-list.component';
import { StationListComponent } from './components/employee/station-list/station-list.component';
import { EmployeeListComponent } from './components/employee/employee-list/employee-list.component';
import { BillDetailComponent } from './reusables/bill-detail/bill-detail.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { ChangePasswordFormComponent } from './reusables/change-password-form/change-password-form.component';
import { PlanOverviewComponent } from './components/plan-overview/plan-overview.component';

@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    UserDisplayComponent,
    StartComponent,
    LoginComponent,
    RegisterComponent,
    PaymentSelectComponent,
    BookingComponent,
    HistoryComponent,
    BookingDetailPanelComponent,
    CarbonaraIconComponent,
    StationSelectComponent,
    OverviewComponent,
    WelcomeComponent,
    PlanSelectComponent,
    PlanFormComponent,
    CarFormComponent,
    AddressInputComponent,
    CarFormComponent,
    CarclassFormComponent,
    CartypeFormComponent,
    StationFormComponent,
    EmployeeFormComponent,
    UserdataChangeFormComponent,
    ChangeUserdateComponent,
    UserListComponent,
    CarListComponent,
    CartypeListComponent,
    PlanListComponent,
    CarclassListComponent,
    StationListComponent,
    EmployeeListComponent,
    BillDetailComponent,
    ChangePasswordFormComponent,
    PlanOverviewComponent
    ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatChipsModule,
    MatCommonModule,
    MatDividerModule,
    MatStepperModule,
    MatFormFieldModule,
    MatInputModule,
    MatGridListModule,
    MatAutocompleteModule,
    MatCheckboxModule,
    MatCardModule,
    MatSelectModule,
    MatExpansionModule,
    MatProgressBarModule,
    MatTabsModule,
    MatButtonToggleModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatTableModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      enabled: environment.production,
      // Register the ServiceWorker as soon as the application is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    })
  ],
  providers: [
    AuthenticationService, UtilityService, BookingService, UserService, DatabaseService, AdminService, CarService,
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: {appearance: 'outline'} },
    { provide: STEPPER_GLOBAL_OPTIONS, useValue: { showError: true } },
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true },
    { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: { duration: 3000 }}
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
