import { Component, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { PaymentMethod } from 'src/app/model/user';

@Component({
  selector: 'cb-payment-select',
  templateUrl: './payment-select.component.html',
  styleUrls: ['./payment-select.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    multi: true,
    useExisting: PaymentSelectComponent
  }]
})
export class PaymentSelectComponent implements OnInit, ControlValueAccessor {



  //Icons at https://github.com/mpay24/payment-logos
  availablePaymentMethods : PaymentMethodOption[] = [
    { name: "Paypal", imageUrl: "./assets/images/payments/paypal.png", method: PaymentMethod.Paypal },
    { name: "SEPA Lastschrift", imageUrl: "./assets/images/payments/sepa.png", method: PaymentMethod.Sepa },
    { name: "Maestro", imageUrl: "./assets/images/payments/maestro.png", method: PaymentMethod.Maestro },
    { name: "Mastercard", imageUrl: "./assets/images/payments/mastercard.png", method: PaymentMethod.Mastercard },
    { name: "Visa", imageUrl: "./assets/images/payments/visa.png", method: PaymentMethod.Visa },
    { name: "GiroPay", imageUrl: "./assets/images/payments/giropay.png", method: PaymentMethod.Giropay },
  ];

  constructor() { }

  onChangeFn = (method: PaymentMethod) => {};
  onTouchedFn = () => {};

  selectedIndex = -1;
  value: PaymentMethod | null = null;
  @Input() disabled: boolean = false;

  writeValue(method: PaymentMethod): void {
    if (method != null)
    {
      this.selectedIndex = this.availablePaymentMethods.findIndex(m => m.method == method);
      this.value = this.selectedIndex > 0 ? method : null;
    }
  }
  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }

  ngOnInit(): void {
  }

  select()
  {
    this.value = this.availablePaymentMethods[this.selectedIndex].method ?? null;
    if (this.value)
    {
      this.onTouchedFn();
      this.onChangeFn(this.value);
    }
  }

}


export class PaymentMethodOption {
  public name: string = "";
  public imageUrl: string = "";
  public method?: PaymentMethod;
}
