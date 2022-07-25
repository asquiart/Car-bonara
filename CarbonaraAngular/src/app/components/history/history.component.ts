import { Component, OnInit } from '@angular/core';
import { Booking } from 'src/app/model/booking';
import { BookingService } from 'src/app/services/booking.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrls: ['./history.component.scss']
})
export class HistoryComponent implements OnInit {

  constructor(
    public bookingService: BookingService,
    private utilityService: UtilityService
  ) { }

  oldBookings: Booking[] | null = null
  currentBookings: Booking[] | null = null
  futureBookings: Booking[] | null = null

  ngOnInit(): void {
    this.updateHistory();
  }

  updateHistory() {
    this.bookingService.loadBookingHistory().subscribe(bookings => {
      this.oldBookings = [];
      this.currentBookings = [];
      this.futureBookings = [];

      let currentUtcDate = this.utilityService.dateToUTC(new Date(Date.now()));

      bookings.forEach(b => {
        if (b.returned || b.cancelled)
        {
          this.oldBookings?.push(b);
        } else if (b.startTime > currentUtcDate.getTime())
        {
          this.futureBookings?.push(b);
        } else
        {
          this.currentBookings?.push(b);
        }
      });
    });
  }

}
