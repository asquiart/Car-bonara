namespace CarbonaraWebAPI.Model.DTO
{
    public class AddCarDTO
    {
        public AddCarDTO(CarDTO car, StationDTO firstStation, long firstAvailableTime)
        {
            this.Car = car;
            this.Station = firstStation;
            this.Time = firstAvailableTime;
        }


        public AddCarDTO()
        { }

        public CarDTO Car { get; set; }
        public StationDTO Station { get; set; }
        public long Time { get; set; }
    }
}

