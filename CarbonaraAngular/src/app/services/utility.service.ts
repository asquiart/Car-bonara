import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UtilityService {

  constructor() { }

  getDateString(date: Date) : string
  {
    return this.paddingLeft(date.getDate(), 2) + "." + this.paddingLeft(date.getMonth() + 1, 2) + "." + date.getFullYear();
  }

  getTimeString(date: Date) : string {
    return this.paddingLeft(date.getHours(), 2) + ":" + this.paddingLeft(date.getMinutes(), 2);
  }

  getDateTimeString(date: Date) : string
  {
    return this.getDateString(date) + " - " + this.getTimeString(date);
  }

  getISODateTimeString(date: Date) : string {
    return this.paddingLeft(date.getFullYear(), 2) + "-" + this.paddingLeft(date.getMonth(), 2) + "-" + this.paddingLeft(date.getDate(), 2) + "T00:00";// + this.getTimeString(date);
  }

  paddingLeft(number: number, minLength: number) : string
  {
    let n = number.toString();
    while (n.length < minLength)
      n = "0" + n;
    return n;
  }

  dateToUTC(date: Date) : Date
  {
    return date;
  //  let utcDate = new Date(date.getTime() + (new Date()).getTimezoneOffset()*60000);
  //  return utcDate;
  }

  UTCtoLocal(utcDate: Date) : Date {
    return utcDate;
 //   let localDate = new Date(utcDate.getTime() - (new Date()).getTimezoneOffset()*60000);
  //  return localDate;
  
  }

  
}
