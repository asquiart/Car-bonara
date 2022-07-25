export class TimeSpan
{
    private span!: Date;

    constructor(startDate: number, endDate: number)
    {
        this.span = new Date(endDate - startDate);
    }

    public static fromDates(startDate: Date, endDate: Date) : TimeSpan
    {
        return new TimeSpan(startDate.valueOf(), endDate.valueOf());
    }

    public toString() : string {
        let string: string = "";
        let days: string = ""; let hours: string = ""; let minutes: string = "";

        if (this.span.getDate() > 1)
        {
            let valDays = (this.span.getDate() - 1);
            days = valDays + " Tag" + (valDays > 1 ? "e" : "");
            string = days;
        }
        
        if (this.span.getHours() > 1)
        {
            let valHours = this.span.getHours() - 1;
            hours = valHours + " Stunde" + (valHours > 1 ? "n" : "");
            string += (string.length > 0 ? ", " : "") + hours;
        }
        
        if (this.span.getMinutes() > 0)
        {
            let valMinutes = this.span.getMinutes();
            minutes = valMinutes  + " Minute" + (valMinutes > 1 ? "n" : "");
            string += (string.length > 0 ? ", " : "") + minutes;
        }

        return string;
    }


}