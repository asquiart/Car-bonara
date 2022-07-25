import { Address } from "./address";
import { Person } from "./person";
import { Plan } from "./plan";

export class User {
    constructor (
        public id: number,
        public person: Person,
        public plan: Plan,
        public address: Address,
        public driverlicenseNumber: string,
        public state: UserState,
        public cardId: number,
        public payment: PaymentMethod
    )
    {}
}

export enum UserState { 
    Unauthorized,
    Authorized,
    Locked
}

export enum PaymentMethod { 
    Paypal,
    Sepa, 
    Visa, 
    Mastercard, 
    Maestro, 
    Giropay 
}