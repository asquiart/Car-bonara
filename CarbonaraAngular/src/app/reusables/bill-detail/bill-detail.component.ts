import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Bill } from 'src/app/model/Bill';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-bill-detail',
  templateUrl: './bill-detail.component.html',
  styleUrls: ['./bill-detail.component.scss']
})
export class BillDetailComponent implements OnInit {

  form!: FormGroup;



  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Bill,
    private utilityService: UtilityService,
    private dialogRef: MatDialogRef<BillDetailComponent>,
    formBuilder: FormBuilder
  ) { 

    
   
  }

  working: boolean = false;

  
  displayedColumns : string[] = [
    "position",
    "amount",
    "price",
    "totalAmount"
  ]


  formatNumber(num : number) : string
  {
    return (Math.round(num * 100) / 100).toString();
  }

  formatDate(num: number):string
  {
   let time = this.utilityService.UTCtoLocal(new Date(num));
   return this.utilityService.getDateString(time);

  }
  ngOnInit(): void {
  }

  
  close()
  {
    this.dialogRef.close(null);
  }
}


