import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Cartype } from 'src/app/model/cartype';
import { DatabaseService } from 'src/app/services/database.service';


@Component({
  selector: 'cb-cartype-form',
  templateUrl: './cartype-form.component.html',
  styleUrls: ['./cartype-form.component.scss']
})
export class CartypeFormComponent implements OnInit {

  form!: FormGroup;


  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Cartype,
    private dialogRef: MatDialogRef<CartypeFormComponent>,
    formBuilder: FormBuilder,
   public databaseService : DatabaseService

  ) { 

    let defaultData : Cartype = this.data ?? Cartype.prototype;

    this.form = formBuilder.group({
      name: [defaultData.name, [Validators.required, Validators.minLength(1)]],
      manufacturer: [defaultData.manufacturer, [Validators.required ,Validators.minLength(1)]],
      fueltype: [defaultData.fueltype, [Validators.required,Validators.minLength(1)]],
      carclass: [defaultData.carclass?.id, [Validators.required]],
      
    });
  }

  working: boolean = false;

  ngOnInit(): void {
  }

  onSubmit() {
    let value : Cartype = Cartype.prototype;
    Object.assign(value, this.form.value);
    value.carclass = this.databaseService.carclasses.find(c => c.id == this.form.value.carclass)!;
    this.dialogRef.close(value);
  }
  cancel()
  {
    this.dialogRef.close(null);
  }
}
