import { BreakpointObserver } from '@angular/cdk/layout';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { StepperOrientation } from '@angular/material/stepper';
import { map, Observable } from 'rxjs';
import { ValidateNumber } from 'src/app/forms/number-validator';
import { Address } from 'src/app/model/address';
import { Person } from 'src/app/model/person';
import { Plan } from 'src/app/model/plan';
import { PaymentMethod, User } from 'src/app/model/user';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { RegistrationComposite, UserService } from 'src/app/services/user.service';
import { ValidateLength } from '../../forms/length-validator';
import { carbonaraPasswordValidator, checkPassword } from '../../forms/password-validator';
import { ChangePasswordFormComponent } from '../change-password-form/change-password-form.component';


@Component({
  selector: 'cb-userdata-change-form',
  templateUrl: './userdata-change-form.component.html',
  styleUrls: ['./userdata-change-form.component.scss']
})
export class UserdataChangeFormComponent implements OnInit {

  @ViewChild("licenseUpload") licenseUpload!: HTMLInputElement;

  registerForm!: FormGroup;
  personalFormGroup!: FormGroup;
  contactFormGroup!: FormGroup;
  paymentFormGroup!: FormGroup;



  salutations = ["Herr", "Frau", "Hallo", "Moin"];
  filteredSalutations!: Array<string>;
  titles = ["Dr.", "Prof.", "Dipl.", "M.", "B."];
  filteredTitles!: Array<string>;

  selectedLicenseImage: File | null = null;


  constructor(
    private formBuilder: FormBuilder,
    private breakpointObserver: BreakpointObserver,
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private dialogService: MatDialog,
  
  ) {

    userService.currentUserData().subscribe(user => 
      {
      this.registerForm = formBuilder.group({
        personalFormGroup: formBuilder.group({
          title: [user?.person.title],
          formOfAddress: [user?.person.formOfAddress],
          firstname: [user?.person.firstname, [Validators.required, Validators.minLength(2)]],
          lastname: [user?.person.lastname, [Validators.required, Validators.minLength(2)]]
        }),
        contactFormGroup: formBuilder.group({
          email: [user?.person.email, [Validators.required, Validators.email, Validators.maxLength(50)]],
          address: [user?.address, Validators.required]
        }),
        paymentFormGroup: formBuilder.group({
          paymentMethod: [user?.payment, Validators.required],
          driverlicenseNumber: [user?.driverlicenseNumber, Validators.required],
          plan: [user?.plan, Validators.required]
        }),
      });
      this.personalFormGroup = this.registerForm.controls['personalFormGroup'] as FormGroup;
      this.contactFormGroup = this.registerForm.controls['contactFormGroup'] as FormGroup;
      this.paymentFormGroup = this.registerForm.controls['paymentFormGroup'] as FormGroup;


      //Filter salutations
      this.filteredSalutations = this.salutations;
      this.registerForm.get("personalFormGroup.formOfAddress")?.valueChanges.subscribe((value: string) => {
        let lowerValue = value?.toLowerCase() ?? "";
        this.filteredSalutations = this.salutations.filter(v => v.toLowerCase().includes(lowerValue));
      });
      //Filter titles
      this.filteredTitles = this.titles;
      this.registerForm.get("personalFormGroup.title")?.valueChanges.subscribe((value: string) => {
        let lowerValue = value?.toLowerCase() ?? "";
        this.filteredTitles = this.titles.filter(v => v.toLowerCase().includes(lowerValue));
      });

    });


  }

  ngOnInit(): void {
  }


  changePassword() {
    this.dialogService.open(ChangePasswordFormComponent, {  });

  }

  saveChanges() {

    //Person
    let person = Person.prototype;
    person.email = this.contactFormGroup.value.email as string;
    person.firstname = this.personalFormGroup.value.firstname as string;
    person.lastname = this.personalFormGroup.value.lastname as string;
    person.title = this.personalFormGroup.value.title as string;
    person.formOfAddress = this.personalFormGroup.value.formOfAddress as string;

    //User
    let user = User.prototype;

    user.address = this.contactFormGroup.value.address as Address;
    user.driverlicenseNumber = this.paymentFormGroup.value.driverlicenseNumber as string;
    user.payment = this.paymentFormGroup.value.paymentMethod as PaymentMethod;
    user.person = person;
    user.plan = this.paymentFormGroup.value.plan as Plan;

    console.warn(user);

    this.userService.updateUser(user).subscribe();
  }

}
