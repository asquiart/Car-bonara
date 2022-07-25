import { Component, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Plan } from 'src/app/model/plan';
import { DatabaseService } from 'src/app/services/database.service';

@Component({
  selector: 'cb-plan-select',
  templateUrl: './plan-select.component.html',
  styleUrls: ['./plan-select.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: PlanSelectComponent
    }
  ]
})
export class PlanSelectComponent implements OnInit, ControlValueAccessor {

  constructor(
    public databaseService: DatabaseService
  ) { }

  onTouchedFn = () => { };
  onChangeFn = (plan: Plan) => {};
  @Input() disabled: boolean = false;

  selectedId = -1;


  writeValue(plan: Plan): void {
    if (plan != null)
    {
      this.selectedId = plan.id;
    }
  }
  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }

  ngOnInit(): void {
    this.databaseService.loadPlans();
  }

  select()
  {
    let selectedPlan = this.databaseService.plans.find(p => p.id == this.selectedId);
    if (selectedPlan)
    { 
      this.onChangeFn(selectedPlan);
      this.onTouchedFn();
    }
  }

}
