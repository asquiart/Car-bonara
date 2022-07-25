import { Person } from "./person";

export class Login {
    constructor (
        public token: string,
        public isUser: boolean, // true -> User; false -> Employee
        public person: Person
    ) {}
}