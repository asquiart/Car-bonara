import { BreakpointObserver } from '@angular/cdk/layout';
import { Component, Input, OnInit, ViewChild } from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { StepperOrientation } from '@angular/material/stepper';
import { map, Observable } from 'rxjs';
import { ValidateNumber } from 'src/app/forms/number-validator';
import { Address } from 'src/app/model/address';
import { Person } from 'src/app/model/person';
import { Plan } from 'src/app/model/plan';
import { PaymentMethod, User } from 'src/app/model/user';
import { RegistrationComposite, UserService } from 'src/app/services/user.service';
import { ValidateLength } from '../../forms/length-validator';
import { carbonaraPasswordValidator, checkPassword } from '../../forms/password-validator';
import { PlanOverviewComponent } from '../plan-overview/plan-overview.component';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  @ViewChild("licenseUpload") licenseUpload!: HTMLInputElement;

  registerForm!: FormGroup;
  personalFormGroup!: FormGroup;
  accountFormGroup!: FormGroup;
  addressFormGroup!: FormGroup;
  licenseFormGroup!: FormGroup;
  paymentFormGroup!: FormGroup;
  planFormGroup!: FormGroup;
  stepperOrientation!: Observable<StepperOrientation>;

  salutations = [ "Herr", "Frau", "Hallo", "Moin" ];
  filteredSalutations! : Array<string>;
  titles = [ "Dr.", "Prof.", "Dipl.", "M.", "B." ];
  filteredTitles! : Array<string>;

  selectedLicenseImage: File | null = null;

  constructor(
    private formBuilder: FormBuilder,
    private breakpointObserver: BreakpointObserver,
    private userService: UserService,
    private dialogService: MatDialog
    ) { 
      

      this.registerForm = formBuilder.group({
        personalFormGroup: formBuilder.group({
          title: [''],
          formOfAddress: ['', Validators.minLength(0)],
          firstname: ['', [Validators.required, Validators.minLength(2)]],
          lastname: ['', [Validators.required, Validators.minLength(2)]]
        }),
        accountFormGroup: formBuilder.group({
          email: ['', [Validators.required, Validators.email, Validators.maxLength(50)]],
          password: ['', [Validators.required, carbonaraPasswordValidator()]]
        }),
        addressFormGroup: formBuilder.group({
          address: [null, Validators.required]
        }),
        licenseFormGroup: formBuilder.group({
          submitLicenseLater: [false],
          driverlicenseNumber: ['', Validators.required]
        }),
        paymentFormGroup: formBuilder.group({
          paymentMethod: [null, Validators.required]
        }), planFormGroup: formBuilder.group({
          plan: [null, Validators.required]
        })
      });
      this.personalFormGroup = this.registerForm.controls['personalFormGroup'] as FormGroup;
      this.accountFormGroup = this.registerForm.controls['accountFormGroup'] as FormGroup;
      this.addressFormGroup = this.registerForm.controls['addressFormGroup'] as FormGroup;
      this.licenseFormGroup = this.registerForm.controls['licenseFormGroup'] as FormGroup;
      this.paymentFormGroup = this.registerForm.controls['paymentFormGroup'] as FormGroup;
      this.planFormGroup = this.registerForm.controls['planFormGroup'] as FormGroup;

      //Filter salutations
      this.filteredSalutations = this.salutations;
      this.registerForm.get("personalFormGroup.formOfAddress")?.valueChanges.subscribe((value : string) => {
        let lowerValue = value?.toLowerCase() ?? "";
        this.filteredSalutations = this.salutations.filter(v => v.toLowerCase().includes(lowerValue));
      });
      //Filter titles
      this.filteredTitles = this.titles;
      this.registerForm.get("personalFormGroup.title")?.valueChanges.subscribe((value : string) => {
        let lowerValue = value?.toLowerCase() ?? "";
        this.filteredTitles = this.titles.filter(v => v.toLowerCase().includes(lowerValue));
      });


      this.stepperOrientation = breakpointObserver
      .observe('(min-width: 1000px)')
      .pipe(map(({matches}) => (matches ? 'horizontal' : 'vertical')));
    }

  ngOnInit(): void {
  }

  getPasswordErrors() : string {
    let passwordControl = this.accountFormGroup.get('password');
    let passwordErrors = checkPassword(passwordControl?.value as string);
    if (!passwordErrors)
      return "";
    else return passwordErrors.errorString;
  }

  selectLicenseFile(event: any)
  {
    this.selectedLicenseImage = event.target.files.length > 0 ? event.target.files[0] : null;
    this.licenseFormGroup.get('submitLicenseLater')?.setValue(false);
  }

  register() {

    //Person
    let person = Person.prototype;
    Object.assign(person, this.personalFormGroup.value);
    person.email = this.accountFormGroup.value.email as string;

    //User
    let user = User.prototype;
    user.person = person;
    user.plan = this.planFormGroup.value.plan as Plan;
    user.address = this.addressFormGroup.value.address as Address;
    user.plan = this.planFormGroup.value.plan as Plan;
    user.payment = this.paymentFormGroup.value.paymentMethod as PaymentMethod;
    user.driverlicenseNumber = this.licenseFormGroup.value.driverlicenseNumber;

    console.warn(user);
    let password = this.accountFormGroup.value.password as string;
    this.userService.registerUser(new RegistrationComposite(user, password));
  }

  openPlans() {
    this.dialogService.open(PlanOverviewComponent, {
      minWidth: 500,
    })
  }
}
