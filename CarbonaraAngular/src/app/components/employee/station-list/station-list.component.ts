import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Component, OnInit } from '@angular/core';
import { Station } from 'src/app/model/station';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DatabaseService } from 'src/app/services/database.service';
import { StationFormComponent } from 'src/app/reusables/database/station-form/station-form.component';

@Component({
  selector: 'app-station-list',
  templateUrl: './station-list.component.html',
  styleUrls: ['./station-list.component.scss']
})
export class StationListComponent implements OnInit {

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
    const dialog = this.dialogService.open(StationFormComponent, { data: null });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        this.databaseService.addStation(result as Station).subscribe({
          next: () => {
            this.databaseService.loadStations();
          },
          error: () => {
            this.snackbarService.open("Station konnte nicht hinzugefügt werden");
          }
        });
      }
    });
  }

  change(station :Station)
  {
    const dialog = this.dialogService.open(StationFormComponent, { data: station });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        this.databaseService.updateStation(station.id, result as Station).subscribe({
          next: () => {
            this.databaseService.loadStations();
          },
          error: () => {
            this.snackbarService.open("Station konnte nicht geändert werden");
          }
        });
      }
    });
  }

}
