import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Component, OnInit } from '@angular/core';
import { Plan } from 'src/app/model/plan';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { DatabaseService } from 'src/app/services/database.service';
import { PlanFormComponent } from 'src/app/reusables/database/plan-form/plan-form.component';

@Component({
  selector: 'app-plan-list',
  templateUrl: './plan-list.component.html',
  styleUrls: ['./plan-list.component.scss']
})
export class PlanListComponent implements OnInit {

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
    const dialog = this.dialogService.open(PlanFormComponent, { data: null });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        this.databaseService.addPlan(result as Plan).subscribe({
          next: () => {
            this.databaseService.loadPlans();
          },
          error: () => {
            this.snackbarService.open("Tarif konnte nicht hinzugefügt werden");
          }
        });
      }
    });
  }

  change(plan :Plan)
  {
    const dialog = this.dialogService.open(PlanFormComponent, { data: plan });
    dialog.afterClosed().subscribe(result => {
      if (result)
      {
        this.databaseService.updatePlan(plan.id, result as Plan).subscribe({
          next: () => {
            this.databaseService.loadPlans();
          },
          error: () => {
            this.snackbarService.open("Tarif konnte nicht geändert werden");
          }
        });
      }
    });
  }


}
