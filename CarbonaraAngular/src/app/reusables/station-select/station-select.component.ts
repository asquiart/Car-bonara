import { Component, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Station } from 'src/app/model/station';
import { DatabaseService } from 'src/app/services/database.service';

@Component({
  selector: 'cb-select-station',
  templateUrl: './station-select.component.html',
  styleUrls: ['./station-select.component.scss'],
  providers: [
    { 
      provide: NG_VALUE_ACCESSOR,
      multi: true,
      useExisting: StationSelectComponent
    }
  ]
})
export class StationSelectComponent implements OnInit, ControlValueAccessor {

  constructor(
    public databaseService: DatabaseService
  ) { }

  onChangeFn = (station: Station) => {};
  onTouchedFn = () => {};
  @Input() disabled: boolean = false;

  writeValue(station: Station): void {
    this.selectedStation = station;
  }
  registerOnChange(fn: any): void {
    this.onChangeFn = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }


  @Input() label: string = "Station";

  cities: Map<string, Station[]> = new Map<string, Station[]>();

  stations: StationGroup[] = [];
  selectedStation: Station | null = null;

  ngOnInit(): void {
    this.databaseService.stationsLoadedObservable.subscribe(stations => {
      this.loadStations(stations)
    });

    if (!this.databaseService.stations || this.databaseService.stations.length < 1)
    {
      this.databaseService.loadStations();
    } else
    {
      this.loadStations(this.databaseService.stations);
    }
    
  }

  loadStations(stations: Station[])
  {
    this.processStations(this.databaseService.stations);
    this.stations = this.getStationGroups();
  }

  processStations(stations: Station[])
  {
    this.cities.clear();
    for (let station of stations)
    {
      let key = station.address.city;
      let keyObject : Station[] = [];
      if (this.cities.has(key))
      {
        keyObject = this.cities.get(key)!;
      }
      
      keyObject.push(station);
      keyObject.sort((a, b) => a.name.localeCompare(b.name));
      if (!this.cities.has(key))
        this.cities.set(key, keyObject);
    }
  }

  getStationGroups() : StationGroup[]
  {
    let keys: string[] = [];
    for (let key of this.cities.keys())
    {
      keys.push(key);
    }

    let stationGroups: StationGroup[] = [];
    for (let key of keys)
    {
      let stationList = this.cities.get(key);
      if (stationList)
        stationGroups.push({ city: key, stations: stationList });
    }
    //return [];
    return stationGroups.sort((a, b) => a.city.localeCompare(b.city));
  }

  onSelect()
  {
    if (this.selectedStation)
    {
      this.onTouchedFn();
      this.onChangeFn(this.selectedStation);
    }
  }

  getAddressTitle(station: Station) : string {
    let address = station.address;
    return address.street + " " + address.number + ", " + address.zip + " " + address.city;
  }

}

class StationGroup {
  city: string = "";
  stations: Station[] = [];
}
