import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Observer, Subject, Subscriber } from 'rxjs';
import { Car } from '../model/car';
import { CarClass } from '../model/carclass';
import { Cartype } from '../model/cartype';
import { Plan } from '../model/plan';
import { Station } from '../model/station';
import { AuthenticationService } from './authentication.service';
import {  CarStationTimeComposit } from './car.service';
import { NetworkService } from './network.service';

@Injectable({
  providedIn: 'root'
})
export class DatabaseService {

  static stationsUrl : string = "/api/stationDatabase";
  static carclassURL : string = "/api/carclassDatabase";
  static cartypeURL : string = "/api/cartypeDatabase";
  static carUrl : string = "/api/carDatabase";
  static planURL : string = "/api/planDatabase";

  constructor(
    private http: HttpClient,
    private authenticationService : AuthenticationService,
    private networkService: NetworkService
  ) { 
    this.initialize();
    

    authenticationService.loggedOutEvent.subscribe(() => this.onLogout());
    networkService.networkOnlineAgainEvent.subscribe(() => {
      this.initialize();
    })
  }

  public stationsLoadedObservable = new Subject<Station[]>();
  public loadingStations: boolean = false;

  public stations: Station[] = [];
  public carclasses: CarClass[] = [];
  public cars: Car[] = [];
  public cartypes: Cartype[] = [];
  public plans: Plan[] = [];

  private onLogout(){
    this.cars = [];
  }
  
  private initialize()
  {
    this.loadStations();
    this.loadCarclasses();
    this.loadCartypes();
    this.loadStations();
    this.loadPlans();
  }


  public loadStations()
  {
    if (!this.loadingStations)
    {
      this.loadingStations = true;
      this.http.get<Station[]>(DatabaseService.stationsUrl + "/getall").subscribe(stations => {
        this.stations = stations;
        this.loadingStations = false;
        this.stationsLoadedObservable.next(this.stations);
      });
    }
  }


 

  public getStation(id : number) : Observable<Station>
  {
    return this.http.get<Station>(DatabaseService.stationsUrl + "/get/" + id  );
  }
  public updateStation(id : number ,newData : Station) : Observable<any>
  {
    return this.http.put(DatabaseService.stationsUrl + "/change/" + id , newData);
  }
  public addStation(newData : Station) : Observable<any>
  {
    return this.http.post(DatabaseService.stationsUrl + "/add" , newData);
  }





  public loadPlans()
  {
    this.http.get<Plan[]>(DatabaseService.planURL + "/getall").subscribe(plans => this.plans = plans);
  }
  public getPlan(id : number) : Observable<Plan>
  {
    return this.http.get<Plan>(DatabaseService.planURL + "/get/" + id  );
  }
  public updatePlan(id : number ,newData : Plan) : Observable<any>
  {
    return this.http.put(DatabaseService.planURL + "/change/" + id , newData);
  }
  public addPlan(newData : Plan) : Observable<any>
  {
    return this.http.post(DatabaseService.planURL + "/add" , newData);
  }




  public loadCars()
  {
    this.http.get<Car[]>(DatabaseService.carUrl + "/getall").subscribe(cars => this.cars = cars);
  }
  public getCar(id : number) : Observable<Car>
  {
    return this.http.get<Car>(DatabaseService.carUrl + "/get/" + id  );
  }
  public updateCar(id : number ,newData : Car) : Observable<any>
  {
    return this.http.put(DatabaseService.carUrl + "/change/" + id , newData);
  }
  
  public addCar(newData : CarStationTimeComposit) : Observable<any>
  {
    return this.http.post(DatabaseService.carUrl + "/add", newData);
  }




  public loadCartypes()
  {
    this.http.get<Cartype[]>(DatabaseService.cartypeURL + "/getall").subscribe(cartypes => this.cartypes = cartypes);
  }
  public getCartype(id : number) : Observable<Cartype>
  {
    return this.http.get<Cartype>(DatabaseService.cartypeURL + "/get/" + id  );
  }
  public updateCartype(id : number ,newData : Cartype) : Observable<any>
  {
    return this.http.put(DatabaseService.cartypeURL + "/change/" + id , newData);
  }
  public addCartype(newData : Cartype) : Observable<any>
  {
    return this.http.post(DatabaseService.cartypeURL + "/add" , newData);
  }



  public loadCarclasses()
  {
    this.http.get<CarClass[]>(DatabaseService.carclassURL + "/getall").subscribe(carclasses => this.carclasses = carclasses);
  }
  public getCarclass(id : number) : Observable<CarClass>
  {
    return this.http.get<CarClass>(DatabaseService.carclassURL + "/get/" + id  );
  }
  public updateCarclass(id : number ,newData : CarClass) : Observable<any>
  {
    return this.http.put(DatabaseService.carclassURL + "/change/" + id , newData);
  }
  public addCarclass(newData : CarClass) : Observable<any>
  {
    return this.http.post(DatabaseService.carclassURL + "/add", newData);
  }
}
