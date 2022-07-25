import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CarClass } from 'src/app/model/carclass';
import { CarclassFormComponent } from 'src/app/reusables/database/carclass-form/carclass-form.component';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DatabaseService } from 'src/app/services/database.service';


@Component({
  selector: 'app-carclass-list',
  templateUrl: './carclass-list.component.html',
  styleUrls: ['./carclass-list.component.scss']
})
export class CarclassListComponent implements OnInit {


  constructor(
    private authenticationService: AuthenticationService,
    public databaseService: DatabaseService,
    private dialogService: MatDialog,
    private snackbarService: MatSnackBar
  ) {
   
  }

 
  ngOnInit(): void {
    this.databaseService.loadCarclasses();
  }


  addNew()
  {
    const dialog = this.dialogService.open(CarclassFormComponent, { data: null });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        this.databaseService.addCarclass(result as CarClass).subscribe({
          next: () => {
            this.databaseService.loadCarclasses();
          },
          error: () => {
            this.snackbarService.open("Fahrzeugklasse konnte nicht hinzugefügt werden");
          }
        });
      }
    });
  }

  change(carclass :CarClass)
  {
    const dialog = this.dialogService.open(CarclassFormComponent, { data: carclass });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        this.databaseService.updateCarclass(carclass.id, result as CarClass).subscribe({
          next: () => {
            this.databaseService.loadCarclasses();
          },
          error: () => {
            this.snackbarService.open("Fahrzeugklasse konnte nicht geändert werden");
          }
        });
      }
    });
  }
}
