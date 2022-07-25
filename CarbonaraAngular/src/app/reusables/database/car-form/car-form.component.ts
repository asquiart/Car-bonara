import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Car, CarStatus, LockStatus } from 'src/app/model/car';
import { CarStationTimeComposit } from 'src/app/services/car.service';
import { DatabaseService } from 'src/app/services/database.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'cb-car-form',
  templateUrl: './car-form.component.html',
  styleUrls: ['./car-form.component.scss']
})
export class CarFormComponent implements OnInit {

  form!: FormGroup;

 
  carStatus = [
    { name: "Auf Station",  enum: CarStatus.onStation },
    { name: "In Bewegung",  enum: CarStatus.moving },
    { name: "Geparkt",  enum: CarStatus.parked },
    { name: "Nicht verfügbar",  enum: CarStatus.unavailable }
  ];

  lockStatus = [
    { name: "Offen",  enum: LockStatus.unlocked },
    { name: "Geschlossen",  enum: LockStatus.locked }
  ];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: CarStationTimeComposit,
    private dialogRef: MatDialogRef<CarFormComponent>,
    formBuilder: FormBuilder,
    public databaseService : DatabaseService,
    private utilityService: UtilityService
  ) { 

    let defaultData : CarStationTimeComposit = this.data ?? CarStationTimeComposit.prototype;
    
  
    this.form = formBuilder.group({
      kilometersDriven: [defaultData.car?.kilometersDriven, [Validators.required]],
      lockStatus: [defaultData.car?.lockStatus, [Validators.required]],
      status: [defaultData.car?.status, [Validators.required]],
      tanklevel: [defaultData.car?.tankLevel, [Validators.required]],
      type: [defaultData.car?.type?.id, [Validators.required]],
      licensePlateNumber: [defaultData.car?.licensePlateNumber, [Validators.required , Validators.minLength(1)]],
      startstation : this.data ? [null] : [null, [Validators.required]],
      time : this.data ? [null] : [null, [Validators.required]],
    });
  }

  working: boolean = false;

  ngOnInit(): void {
  }

  onSubmit() {
    let value : CarStationTimeComposit = CarStationTimeComposit.prototype;
    value.car = Car.prototype;
    value.station = this.form.value.startstation;
    value.time = new Date(this.form.value.time).getTime();
    
    Object.assign(value.car, this.form.value);
    value.car.type = this.databaseService.cartypes.find(t => t.id == this.form.value.type)!;
    this.dialogRef.close(value);
  }
  cancel()
  {
    this.dialogRef.close(null);
  }
}
