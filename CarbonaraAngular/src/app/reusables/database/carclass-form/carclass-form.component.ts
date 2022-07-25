import { DialogRef } from '@angular/cdk/dialog';
import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CarClass } from 'src/app/model/carclass';

@Component({
  selector: 'cb-carclass-form',
  templateUrl: './carclass-form.component.html',
  styleUrls: ['./carclass-form.component.scss']
})
export class CarclassFormComponent implements OnInit {

  form!: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: CarClass,
    private dialogRef: MatDialogRef<CarclassFormComponent>,
    formBuilder: FormBuilder
  ) { 

    let defaultData : CarClass = this.data ?? CarClass.prototype;

    this.form = formBuilder.group({
      name: [defaultData.name, [Validators.required, Validators.minLength(1)]],
      priceFaktor: [defaultData.priceFaktor, [Validators.required]]     
    });
  }

  working: boolean = false;

  ngOnInit(): void {
  }

  onSubmit() {
    let value : CarClass = CarClass.prototype;
    Object.assign(value, this.form.value);
    this.dialogRef.close(value);
  }

  cancel()
  {
    this.dialogRef.close(null);
  }


}
