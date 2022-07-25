import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { Routing } from 'src/app/app-routing.module';
import { ValidateDate } from 'src/app/forms/date-validator';
import { Booking } from 'src/app/model/booking';
import { CarClass } from 'src/app/model/carclass';
import { Station } from 'src/app/model/station';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { BookingService } from 'src/app/services/booking.service';
import { DatabaseService } from 'src/app/services/database.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.scss']
})
export class BookingComponent implements OnInit {

  form!: FormGroup;
  selectedCarClass: CarClass | null = null;
  predictedPrice: number | null = null;


  availableCarClasses: CarClass[] = [];
  loading: boolean = false;

  constructor(
    formBuilder: FormBuilder,
    public bookingService: BookingService,
    public authenticationService: AuthenticationService,
    private utilityService: UtilityService,
    private router: Router,
    private snackBarService: MatSnackBar
  ) { 
    this.form = formBuilder.group({
      startStation: [null, Validators.required],
      endStation: [null, Validators.required],
      startTime: [null, [Validators.required, ValidateDate()]],
      endTime: [null, [Validators.required, ValidateDate()]]
    });
    this.form.valueChanges.subscribe(chnages => {
      this.valuesChanged();
    });
  }

  ngOnInit(): void {
  }

  getBooking() : Booking | null {
    if (this.form.valid)
    {
      let booking = Booking.prototype;
      Object.assign(booking, this.form.value);
      booking.startTime = new Date(booking.startTime).getTime(); // this.utilityService.dateToUTC(new Date(booking.startTime)).getTime();
      booking.endTime = new Date(booking.endTime).getTime();
      return booking;
      
    } else return null;
  }


  

  valuesChanged() {
    this.predictedPrice = null;
    let booking = this.getBooking();
    if (booking) {
      this.availableCarClasses = [];
      this.loading = true;
      console.warn(booking);
    
      this.bookingService.loadAvailableCarclasses(booking).subscribe({
        next: (classes) => {
          this.availableCarClasses = classes;
          },
        error: () => {
        },
        complete: () => {
          this.loading = false;
          this.selectedCarClass = null;
        }
      })
    
    }
  }

  book() {
    let booking = this.getBooking();
    if (booking && this.selectedCarClass)
    {
      booking.carclass = this.selectedCarClass;
      this.bookingService.book(booking).subscribe({
        next: ()=>{
          this.router.navigate([Routing.ROUTE_BOOKING_HISTORY]);
        },
        error: () => {
          this.snackBarService.open("Du bist leider noch nicht dafür freigeschaltet eine Buchung durchzuführen");
        }
      });
    }
  }

  checkBooking()
  {
    this.predictedPrice = null;
     let booking = this.getBooking();
     if (booking && this.selectedCarClass)
    {
      booking.carclass = this.selectedCarClass;
      this.bookingService.isBookingAvailable(booking).subscribe({
        next: (price : number )=>{
          this.predictedPrice = price > 0 ? price : null;
        },
        error: () => {
          this.snackBarService.open("Wir können diese Buchung momentan leider nicht anbieten");
        }
      });
    }
  }

  nowTime() : string {
    let time = new Date().toISOString().slice(0, 16);
    return time;
  }
}
