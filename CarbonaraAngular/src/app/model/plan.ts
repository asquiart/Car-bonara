export class Plan {
    constructor (
        public id: number,
        public name: string,
        public priceWholeDay: number,
        public priceHourDay: number,
        public priceHourNight: number,
        public priceHourOverdue: number,
        public priceKm: number,
        public registrationFee: number
    ) {}
}