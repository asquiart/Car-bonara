import { Person } from "./person";

export class Employee {
    constructor (
        public person: Person,
        public isAdmin: boolean,
    ) {}
}