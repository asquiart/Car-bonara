import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Car, CarStatus, LockStatus } from 'src/app/model/car';
import { CarFormComponent } from 'src/app/reusables/database/car-form/car-form.component';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { CarStationTimeComposit } from 'src/app/services/car.service';
import { DatabaseService } from 'src/app/services/database.service';

@Component({
  selector: 'cb-car-list',
  templateUrl: './car-list.component.html',
  styleUrls: ['./car-list.component.scss']
})
export class CarListComponent implements OnInit {

  constructor(
    public databaseService: DatabaseService,
    private dialogService: MatDialog,
    private snackbarService: MatSnackBar
  ) {
  }

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
  
  ngOnInit(): void {
      this.databaseService.loadCars();
  }

 
  addNew()
  {
    const dialog = this.dialogService.open(CarFormComponent, { data: null });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        console.warn(result);
        this.databaseService.addCar(result as CarStationTimeComposit).subscribe({
          next: () => {
            this.databaseService.loadCars();
          },
          error: () => {
            this.snackbarService.open("Fahrzeug konnte nicht hinzugefügt werden");
          }
        });
      }
    });
  }

  change(car :Car)
  {
    let input = CarStationTimeComposit.prototype;
    input.car = car;
    const dialog = this.dialogService.open(CarFormComponent, { data: input });
    dialog.afterClosed().subscribe(result => {
      let resCar = (result as CarStationTimeComposit)?.car
      if (result && car.id && resCar)
      {
        this.databaseService.updateCar( car.id , resCar).subscribe({
          next: () => {
            this.databaseService.loadCars();
          },
          error: () => {
            this.snackbarService.open("Fahrzeug konnte nicht geändert werden");
          }
        });
      }
    });
  }
}
