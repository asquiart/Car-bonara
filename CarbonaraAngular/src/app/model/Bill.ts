import { User } from "./user";

export class Bill {
    constructor(
        public id: number,
        public billDate: number,
        public user: User,
        public amount: number,
        public notes: string,
        public positions: BillPosition[],
       
    )
    {

    }
}

export class BillPosition {
    constructor(
    
        public item: string,
        public amount: number,
        public pricePerAmount: number,
    )
    {

    }
}
