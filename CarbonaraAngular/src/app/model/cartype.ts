import { CarClass } from "./carclass";

export class Cartype {
    constructor(

        public id: number,
        public fueltype: string,
        public manufacturer: string,
        public name: string,
        public carclass: CarClass
    ) {

    }
}