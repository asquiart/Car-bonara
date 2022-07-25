import { CdkStepperNext } from '@angular/cdk/stepper';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Address } from 'src/app/model/address';
import { Bill } from 'src/app/model/Bill';
import { Booking } from 'src/app/model/booking';
import { Car, LockStatus } from 'src/app/model/car';
import { CarClass } from 'src/app/model/carclass';
import { Station } from 'src/app/model/station';
import { TimeSpan } from 'src/app/model/timespan';
import { BookingService } from 'src/app/services/booking.service';
import { CarService } from 'src/app/services/car.service';
import { UtilityService } from 'src/app/services/utility.service';
import { BillDetailComponent } from '../bill-detail/bill-detail.component';

@Component({
  selector: 'cb-bookingdetails',
  templateUrl: './booking-detail-panel.component.html',
  styleUrls: ['./booking-detail-panel.component.scss']
})
export class BookingDetailPanelComponent implements OnInit {

  constructor(
    public utilityService: UtilityService,
    private bookingService: BookingService,
    private dialogService: MatDialog,
    private snackBarService: MatSnackBar,
    private carService: CarService
  ) {   }


  @Input() booking: Booking | null = null;
  @Output() bookingChanged = new EventEmitter();

  car: Car | null = null;

  ngOnInit(): void {
    console.warn(this.booking);
  
    if (  this.booking?.id)
    {
      this.carService.getCarOfBooking(this.booking?.id).subscribe({
        next: (car) => { this.setCar(car);},
        error: () => {}
        //Macht nichts, wenn Buchung nicht aktiv, etc.
      })
    }
  }

  private setCar(car: Car)
  {
    console.warn(car);
    this.car = car;
  }

  getCarString() : string
  {
    let result = this.booking?.carclass.name;
    if (this.car && this.car.licensePlateNumber)
    {
      result += " [" + this.car.licensePlateNumber + "]";
    }
    return result ?? "";
  }

  getHeaderString() : string {
    if (this.booking)
    {
      let startTime = this.utilityService.UTCtoLocal(new Date(this.booking.startTime));
      return this.utilityService.getDateTimeString(startTime);
    } else return "";
  }


  getBookingDuration(): string {
    if (this.booking)
    {
      let span = new TimeSpan(this.booking.startTime, this.booking.endTime);
      return span.toString();     
    } else return "";
  }

  getStartTime() : string {
    if (this.booking)
    {
      let startTime = this.utilityService.UTCtoLocal(new Date(this.booking.startTime));
      return "(" + this.utilityService.getTimeString(startTime) + ")";
    } else return "";
  }

  getEndTime() : string {
    if (this.booking)
    {
      let endTime = this.utilityService.UTCtoLocal(new Date(this.booking.endTime));
   
      return  this.utilityService.getTimeString(endTime) ;
      
    } else return "";
  }

  getReturnedTime() : string {
    if (this.booking && this.booking.returnedTime)
    {
      let time = this.utilityService.UTCtoLocal(new Date(this.booking.returnedTime));  
      return  this.utilityService.getTimeString(time);
    } else return "";
  }

  isActive() : boolean {
    return !this.booking?.returned 
      && ((this.booking?.startTime ?? 0) < this.utilityService.dateToUTC(new Date()).getTime())
      && !this.booking?.cancelled;
  }

  startBooking() {
    if (this.booking && this.booking.id)
    {
      this.bookingService.startBooking(this.booking.id).subscribe(({
        next: (car) => {
          this.setCar(car);
        },
        error: () => {  this.bookingChanged.emit(); }
      }));

     
    


    }
  }

  endBooking() {
    if (this.booking && this.booking.id)
    {
      this.bookingService.finishBooking(this.booking.id).subscribe(()=> {
        this.bookingChanged.emit(); 
      });
    }
  }

  cancelBooking(){
    if (this.booking  && this.booking.id) {
      this.bookingService.cancelBooking(this.booking.id).subscribe(({
        next: () => {
          if (this.booking )
          this.booking.cancelled = true;
          this.bookingChanged.emit(); 
          this.snackBarService.open("Buchung erfolgreich storniert.");
        },
        error: () => { this.snackBarService.open("Die Buchung konnte leider nicht storniert werden. Vielleicht wird dein Auto an der Zielstation benötigt."); }
        //Macht nichts, wenn Buchung nicht aktiv, etc.
      }));
    }
  }

  isFuture()
  {
   return ((this.booking?.startTime ?? 0) > this.utilityService.dateToUTC(new Date()).getTime());
  }
  showBill(booking: Booking)
  {
    if(booking.id)
    {
      this.bookingService.getBill(booking.id).subscribe({
        next: (bill: Bill) => {
          this.dialogService.open(BillDetailComponent, { data: bill });
        },

      });
    }
    
  }



  toggleLock() {
    if (this.isActive() && this.car && this.booking?.id)
    {
      if (this.car.lockStatus == LockStatus.locked)
      {
        this.carService.unlockCarByBooking(this.booking?.id).subscribe(car => {
          this.setCar(car);
        });
      } else
      {
        this.carService.lockCarByBooking(this.booking?.id).subscribe(car => {
          this.setCar(car);
        });
      }
    }
  }
}
