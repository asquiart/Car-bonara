import { Cartype } from "./cartype";

export class Car {
    constructor(

        public id: number,
        public kilometersDriven: number,
        public tankLevel: number,
        public licensePlateNumber: string,
        public status: CarStatus,
        public lockStatus: LockStatus,
        public type: Cartype,
    ) {

    }
   
}

export enum CarStatus { 
    onStation, 
    moving, 
    parked , 
    unavailable
}

export enum LockStatus { 
    unlocked, 
    locked
}