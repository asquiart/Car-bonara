import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Cartype } from 'src/app/model/cartype';
import { CartypeFormComponent } from 'src/app/reusables/database/cartype-form/cartype-form.component';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DatabaseService } from 'src/app/services/database.service';

@Component({
  selector: 'app-cartype-list',
  templateUrl: './cartype-list.component.html',
  styleUrls: ['./cartype-list.component.scss']
})
export class CartypeListComponent implements OnInit {

 
  constructor(
    private authenticationService: AuthenticationService,
    public databaseService: DatabaseService, 
    private dialogService: MatDialog,
    private snackbarService: MatSnackBar
  ) {
   
  }

 
  ngOnInit(): void {
      this.databaseService.loadCartypes();
  }


  addNew()
  {
    const dialog = this.dialogService.open(CartypeFormComponent, { data: null });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        this.databaseService.addCartype(result as Cartype).subscribe({
          next: () => {
            this.databaseService.loadCartypes();
          },
          error: () => {
            this.snackbarService.open("Fahrzeugmodel konnte nicht hinzugefügt werden");
          }
        });
      }
    });
  }

  change(cartype :Cartype)
  {
    const dialog = this.dialogService.open(CartypeFormComponent, { data: cartype });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        this.databaseService.updateCartype(cartype.id, result as Cartype).subscribe({
          next: () => {
            this.databaseService.loadCartypes();
          },
          error: () => {
            this.snackbarService.open("Fahrzeugmodel konnte nicht geändert werden");
          }
        });
      }
    });
  }

}
