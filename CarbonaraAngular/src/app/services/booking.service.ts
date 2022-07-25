import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { delay, Observable, of, Subscriber } from 'rxjs';
import { Address } from '../model/address';
import { Bill } from '../model/Bill';
import { Booking } from '../model/booking';
import { Car } from '../model/car';
import { CarClass } from '../model/carclass';
import { Station } from '../model/station';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  constructor(
    private authenticationService: AuthenticationService,
    private http: HttpClient,
  ) {
    this.authenticationService.loggedInEvent.subscribe(user => {
      this.initialize();
    });
    this.initialize();
  }

  private initialize() {
  }

  //URLs
  bookingUrl = "/api/booking";



 
  public loadBookingHistory() : Observable<Booking[]> {
    
    return this.http.get<Booking[]>(this.bookingUrl + "/history");
  }

  public loadAvailableCarclasses(booking: Booking) : Observable<CarClass[]> {

    booking.carclass = new CarClass(0, 1, "PLACEHOLDER");
    return this.http.put<CarClass[]>(this.bookingUrl + "/availableCarclasses", booking);
  }

  public isBookingAvailable(booking: Booking): Observable<number> {
    
    return this.http.put<number>(this.bookingUrl + "/available", booking);
  }

  public book(booking: Booking): Observable<any> {

    return this.http.post(this.bookingUrl + "/book", booking);
  }
  public isChangeBookingAllowed(booking: Booking): Observable<boolean> {
    
    return this.http.put<boolean>(this.bookingUrl + "/isChangePossible/"+  booking.id , booking);
  }
  public changeBooking(id : number, newData: Booking) : Observable<any> {
    
    return this.http.put(this.bookingUrl + "/change/"+  id , newData);
  }
  public cancelBooking(id : number) : Observable<any> {
    
    return this.http.put(this.bookingUrl + "/cancel/"+  id , null);
  }

  public startBooking(id : number) : Observable<Car> {
    return this.http.put<Car>(this.bookingUrl + "/start/"+  id , null);
  }

  public finishBooking(id : number) : Observable<any> {
    return this.http.put(this.bookingUrl + "/finish/"+  id , null);
  }

  public loadBooking(id : number) : Observable<Booking> {
    
    return this.http.put<Booking>(this.bookingUrl + "/get/"+  id , null);
  }

  public getBill(bookingId : number) : Observable<Bill> {
    
    return this.http.get<Bill>(this.bookingUrl + "/getBill/"+  bookingId);
  }
}
