import { CarClass } from "./carclass";
import { Station } from "./station";
import { User } from "./user";

export class Booking {

    constructor(
       
        public startTime: number,
        public endTime: number,
        public startStation: Station,
        public endStation: Station,
        public carclass: CarClass
    )
    {}
    public returnedTime?: number;
    public remarks?: string;
    public cancelled?: boolean;
    public returned?: boolean;
    public startKilometers?: number;
    public endKilometers?: number;
    public carLicenseNumber?: string;
    public billAmount?: number;
    public id?: number;
    public user?: User;

    
}