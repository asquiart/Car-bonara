import { Component, OnInit } from '@angular/core';
import { DatabaseService } from 'src/app/services/database.service';

@Component({
  selector: 'cb-plan-overview',
  templateUrl: './plan-overview.component.html',
  styleUrls: ['./plan-overview.component.scss']
})
export class PlanOverviewComponent implements OnInit {

  constructor(
    public databaseService : DatabaseService
  ) { }

  ngOnInit(): void {
  }



  displayedColumns : string[] = [
    "name",
    "registrationFee",
    "priceKm",
    "priceWholeDay",
    "priceHourDay",
    "priceHourNight",
    "priceHourOverdue"
  ]


}
