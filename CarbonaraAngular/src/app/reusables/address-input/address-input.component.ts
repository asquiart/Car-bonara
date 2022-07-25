import { Component, OnInit } from '@angular/core';
import { ControlValueAccessor, FormBuilder, FormGroup, NG_VALUE_ACCESSOR, Validators } from '@angular/forms';
import { ValidateLength } from 'src/app/forms/length-validator';
import { ValidateNumber } from 'src/app/forms/number-validator';
import { Address } from 'src/app/model/address';

@Component({
  selector: 'cb-address-input',
  templateUrl: './address-input.component.html',
  styleUrls: ['./address-input.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: AddressInputComponent
    }
  ]
})
export class AddressInputComponent implements OnInit, ControlValueAccessor {


  form!: FormGroup;
  constructor(
    formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      street: ['', [Validators.required, Validators.minLength(2)]],
      number: ['', [Validators.required, Validators.minLength(1)]],
      zip: ['', [ValidateLength(5), ValidateNumber()]],
      city: ['', [Validators.required, Validators.minLength(2)]]
    });
  }

  onChangeFn = (address: Address) => { };
  onTouchedFn = () => { };
  disabled: boolean = false;

  writeValue(address: Address): void {
    this.load(address)
  }
  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }

  ngOnInit(): void {
  }

  load(address: Address) {
    if (address) {
      this.form.setValue({
        street: address.street,
        number: address.number,
        zip: address.zip,
        city: address.city
      });
    }
  }

  change() {
    this.onTouchedFn();

    let value: Address = Address.prototype;
    Object.assign(value, this.form.value);
    value.country = "Deutschland";
    this.onChangeFn(value);
  }

}
