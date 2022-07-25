import { Address } from "./address";

export class Person {
    constructor (
        public id: number,
        public firstname: string,
        public lastname: string,
        public title: string,
        public formOfAddress: string,

        public lastLogin: number,

        public email: string,


    ) {}
}