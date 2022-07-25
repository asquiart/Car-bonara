import { Address } from "./address";

export class Station {
    constructor(
        public id: number,
        public name: string,
        public address: Address,
        public capacity: number
    )
    {}

    public getStationName() 
    {
        return this.address.street + " " + this.address.number;
    }
}