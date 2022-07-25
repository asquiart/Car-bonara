import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Station } from 'src/app/model/station';
import { DatabaseService } from 'src/app/services/database.service';
@Component({
  selector: 'cb-station-form',
  templateUrl: './station-form.component.html',
  styleUrls: ['./station-form.component.scss']
})
export class StationFormComponent implements OnInit {

  form!: FormGroup;

 

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Station,
    private dialogRef: MatDialogRef<StationFormComponent>,
    formBuilder: FormBuilder,
    public databaseService : DatabaseService
  ) { 

    let defaultData : Station = this.data ?? Station.prototype;

    this.form = formBuilder.group({
      address: [defaultData.address, [Validators.required]],
      capacity: [defaultData.capacity, [Validators.required]],
      name: [defaultData.name, [Validators.required , Validators.minLength(1)]],
      
    });
  }

  working: boolean = false;

  ngOnInit(): void {
  }

  onSubmit() {
    let value : Station = Station.prototype;
    Object.assign(value, this.form.value);
    this.dialogRef.close(value);
  }

  cancel()
  {
    this.dialogRef.close(null);
  }
  
}
