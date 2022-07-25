import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Plan } from 'src/app/model/plan';

@Component({
  selector: 'cb-plan-form',
  templateUrl: './plan-form.component.html',
  styleUrls: ['./plan-form.component.scss']
})
export class PlanFormComponent implements OnInit {

  form!: FormGroup;

  

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Plan,
    private dialogRef: MatDialogRef<PlanFormComponent>,
    formBuilder: FormBuilder
  ) { 

    let defaultData : Plan = this.data ?? Plan.prototype;

    this.form = formBuilder.group({
      name: [defaultData.name, [Validators.required, Validators.minLength(1)]],
      priceWholeDay: [defaultData.priceWholeDay, [Validators.required]],
      priceHourDay: [defaultData.priceHourDay, [Validators.required]],
      priceHourNight: [defaultData.priceHourNight, [Validators.required]],
      priceHourOverdue: [defaultData.priceHourOverdue, [Validators.required]],
      priceKm: [defaultData.priceKm, [Validators.required]],
      registrationFee: [defaultData.registrationFee, [Validators.required]]
    });
  }

  working: boolean = false;

  ngOnInit(): void {
  }

  onSubmit() {
    let value : Plan = Plan.prototype;
    Object.assign(value, this.form.value);
    this.dialogRef.close(value);
  }


  cancel()
  {
    this.dialogRef.close(null);
  }
}
