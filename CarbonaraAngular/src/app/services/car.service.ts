import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Booking } from '../model/booking';
import { Car } from '../model/car';
import { Station } from '../model/station';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class CarService {

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
  static carURL  = "/api/car";


  public lockCarByBooking(bookingId : Number ) : Observable<Car>
  {
    return this.http.put<Car>(CarService.carURL + "/lock/" + bookingId , null);
  }
  public unlockCarByBooking(bookingId : Number ) : Observable<Car>
  {
    return this.http.put<Car>(CarService.carURL + "/unlock/" + bookingId, null);
  }
  public getCarOfBooking(bookingId: number) : Observable<Car>
  {
    return this.http.get<Car>(CarService.carURL + "/car/" + bookingId);
  }

  public getFuel(carId : Number) : Observable<Number>
  {
    return this.http.get<Number>(CarService.carURL + "/fuel/" + carId  );
  }
  public getKilometers(carId : Number) : Observable<Number>
  {
    return this.http.get<Number>(CarService.carURL + "/km/" + carId  );
  }
  public getCurrentBooking(carId : Number) : Observable<Booking>
  {
    return this.http.get<Booking>(CarService.carURL + "/booking/" + carId  );
  }
}

export class CarStationTimeComposit {
  public car: Car | null = null;
  public station : Station | null = null;
  public time : number | null = null;
}