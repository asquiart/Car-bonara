using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Model.DAO;
using CarbonaraWebAPI.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace CarbonaraWebAPI.Services
{
    public class BookingService
    {
        
        private AppDbContext Context;
        private CarService CarService;
        public BookingService(AppDbContext context , CarService carService)
        {
            CarService = carService;
            Context = context;
        }


      
        public bool MakeBooking(BookingDTOIn dto , User user)
        {
            if (!IsBookingAvailable(dto))
                return false;
            Booking booking = new Booking( 
                DateTimeOffset.FromUnixTimeMilliseconds(dto.StartTime).UtcDateTime,
                DateTimeOffset.FromUnixTimeMilliseconds(dto.EndTime).UtcDateTime, 
                DateTime.UtcNow, 
                "", 
                false, 
                false, 
                user, 
                Context.Carclass.Single(c => c.Id == dto.Carclass.Id),
                Context.Station.Single(c => c.Id == dto.Startstation.Id),
                Context.Station.Single(c => c.Id == dto.Endstation.Id),
                null,
                null);

            Context.Add(booking);
            Context.SaveChanges();
            return true;
        }
        public IEnumerable<CarclassDTO> GetAvailableCarClasses(BookingDTOIn dto)
        {

            List<CarclassDTO> typs = new List<CarclassDTO>();

            foreach (Carclass carclass in Context.Carclass.ToArray())
            {
                dto.Carclass = new CarclassDTO( carclass);
                if (IsBookingAvailable(dto))
                    typs.Add(new CarclassDTO( carclass));
            }

            return typs;
        }

        public bool IsBookingAvailable(BookingDTOIn dto)
        {
            if (!IsBookingValid(dto))
                return false;

            //Create a Chronologicaly Sorted List of Events that change the amount of Cars in each Station
            List<BookingEvent> events = CreateBookingEventList();
            Carclass carclass = Context.Carclass.Single(c => c.Id == dto.Carclass.Id);
            events.Add(new BookingEvent(dto.Id, false, true, DateTimeOffset.FromUnixTimeMilliseconds(dto.StartTime).UtcDateTime, Context.Station.Single(c => c.Id == dto.Startstation.Id), carclass));
            events.Add(new BookingEvent(dto.Id, true, false, DateTimeOffset.FromUnixTimeMilliseconds(dto.EndTime).UtcDateTime, Context.Station.Single(c => c.Id == dto.Endstation.Id), carclass));
            events = events.OrderBy(c => c.Time).ToList();
            //


   

            return CheckIfBookingEventListIsValid(events , carclass);
        }

        private List<BookingEvent> CreateBookingEventList()
        {
            List<BookingEvent> events = new List<BookingEvent>();

            foreach (Booking booking in Context.Booking.Where(c => true).Include(c => c.Startstation).Include(c => c.Endstation).Include(c => c.Carclass).ToArray())
            {
                if (!booking.Cancelled)
                {
                    if (booking.Startstation != null)
                        events.Add(new BookingEvent(booking.Id, false, true, booking.Starttime, booking.Startstation, booking.Carclass));
                    if (booking.Endstation != null)
                        events.Add(new BookingEvent(booking.Id, true, false, booking.Endtime, booking.Endstation, booking.Carclass));
                }
            }

            return events;
        }

        public IEnumerable<BookingDTOOut> GetBookingHistory(User user)
        {
            List<BookingDTOOut> list = new List<BookingDTOOut>();
            foreach (Booking booking in Context.Booking.Where(c => c.User.Id == user.Id).Include(c => c.Carclass).Include(c => c.Startstation).ThenInclude(ss => ss.Address).Include(c => c.Endstation).ThenInclude(es => es.Address).Include(b => b.User).ThenInclude(u => u.Person).ToArray().OrderByDescending(b => b.Starttime))
            {
                list.Add(new BookingDTOOut(booking, true));
            }
            return list;

            
        }

        public IEnumerable<BillDTO> GetBills(User user)
        {
            List<BillDTO> list = new List<BillDTO>();
            foreach (Bill bill in Context.Bill.Where(c => c.User.Id == user.Id).Include(c => c.User).Include(c => c.Positions).ToArray())
            {
                list.Add(new BillDTO(bill));
            }
            return list;


        }

        private bool CheckIfBookingEventListIsValid(List<BookingEvent> events , Carclass carclass)
        {
            // Creates a Counter for each Station to keep track over the amount of cars in that station
            Dictionary<int, int> stationCarclassCount = new Dictionary<int, int>();
            Dictionary<int, int> stationTotalCount = new Dictionary<int, int>();
            foreach (Station station in Context.Station.ToList())
            {           
                stationCarclassCount.Add(station.Id, 0);
                stationTotalCount.Add(station.Id, 0);
            }
            //

            //Iterates every Event and Check if the amount of Cars in each Station is valid
            for (int i = 0; i < events.Count; i++)
            {
                BookingEvent currentEvent = events[i];
                int amountOfCarclassInStation =  stationCarclassCount[(currentEvent.Station.Id)];
                int amountOfTotalCarsInStation = stationTotalCount[currentEvent.Station.Id];

                if (currentEvent.AddCar)
                {
                    amountOfTotalCarsInStation++;
                    if (currentEvent.Carclass == carclass)
                        amountOfCarclassInStation++;
                }
                if (currentEvent.RemoveCar)
                {
                    amountOfTotalCarsInStation--;
                    if (currentEvent.Carclass == carclass)
                        amountOfCarclassInStation--;
                }
                if (amountOfCarclassInStation < 0 || amountOfTotalCarsInStation > currentEvent.Station.Capacity)
                    return false;

                stationCarclassCount[(currentEvent.Station.Id)] = amountOfCarclassInStation;
                stationTotalCount[currentEvent.Station.Id]       = amountOfTotalCarsInStation;
                
            }
            //
            return true;
        }
        public bool IsChangeBookingAllowed(Booking old, BookingDTOIn newBooking)
        {
            if (!IsBookingValid(newBooking))
                return false;
            List<BookingEvent> events = CreateBookingEventList();
            events.RemoveAll(c => c.Id == old.Id);

            Carclass? carclass = Context.Carclass.SingleOrDefault(c => c.Id == newBooking.Carclass.Id);
            if (carclass == null)
                return false;
            events.Add(new BookingEvent(newBooking.Id, false, true, DateTimeOffset.FromUnixTimeMilliseconds(newBooking.StartTime).UtcDateTime, Context.Station.Single(c => c.Id == newBooking.Startstation.Id), carclass));
            events.Add(new BookingEvent(newBooking.Id, true, false, DateTimeOffset.FromUnixTimeMilliseconds(newBooking.EndTime).UtcDateTime, Context.Station.Single(c => c.Id == newBooking.Endstation.Id), carclass));
            events = events.OrderBy(c => c.Time).ToList();

            return CheckIfBookingEventListIsValid(events , carclass);
        }
      
        public bool ChangeBooking(Booking old, BookingDTOIn newBooking)
        {
     

            if (IsChangeBookingAllowed(old, newBooking))
            {
                Context.Booking.Update(old);
                old.Endtime = DateTimeOffset.FromUnixTimeMilliseconds(newBooking.EndTime).UtcDateTime;
                old.Starttime = DateTimeOffset.FromUnixTimeMilliseconds(newBooking.StartTime).UtcDateTime;
                old.Startstation = Context.Station.Single(c => c.Id == newBooking.Startstation.Id);
                old.Endstation = Context.Station.Single(c => c.Id == newBooking.Endstation.Id);
                old.Carclass = Context.Carclass.Single(c => c.Id == newBooking.Carclass.Id);
                Context.SaveChanges();
                return true;
            }
            else
                return false;
        }

        public bool IsBookingValid(BookingDTOIn booking)
        {
            var unixMS = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (booking.StartTime > booking.EndTime)
                return false;
            return unixMS < booking.StartTime;
        }


        public bool CancelBooking(Booking booking)
        {
            if(CanCancelBooking(booking))
            {
                Context.Booking.Update(booking);
                booking.Cancelled = true;
                Context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool StartBooking(Booking booking, Car car)
        {

            Booking? currentBooking = CarService.GetCurrentBooking(car);
            Station? currentStation = CarService.GetCurrentStation(car);
            if (booking.Endtime < DateTime.UtcNow )
            {
                booking.Returned = true;
                booking.ReturnedTime = booking.Endtime;
                booking.StartKilometers = -1;
                booking.EndKilometers = -1;
                CarService.LockCar(car);
                CreateBill(booking);
                return false;
            }
            if (currentBooking != null || 
                car.Type.Carclass != booking.Carclass || 
                currentStation != booking.Startstation || 
                booking.Starttime > DateTime.UtcNow || 
                booking.Cancelled || 
                booking.Returned
             )
            {
                return false; 
            }


            Context.Booking.Update(booking);
            booking.Car = car;
            booking.StartKilometers = CarService.GetMillage(car);
            Context.SaveChanges();
            return true;

        }

        public bool CanAddInitialBooking(Station firststation , Carclass carclass , DateTime time )
        {
            List<BookingEvent> events = CreateBookingEventList();
            events.Add(new BookingEvent(0, true, false, time, Context.Station.Single(c => c.Id == firststation.Id), carclass));
            events = events.OrderBy(c => c.Time).ToList();
            return CheckIfBookingEventListIsValid(events, carclass);
        }
        public bool CanCancelBooking(Booking booking)
        {
          
            if (DateTime.UtcNow >= booking.Starttime)
                return false;
            List<BookingEvent> events = CreateBookingEventList();
            events.RemoveAll(c => c.Id == booking.Id);
            events = events.OrderBy(c => c.Time).ToList();
            return CheckIfBookingEventListIsValid(events , booking.Carclass);
        }

        public Booking? GetBooking(int id)
        {
            return Context.Booking.Where(c => c.Id == id).Include(c => c.Carclass).Include(c => c.Startstation).Include(c => c.Endstation).Include(c => c.User).Include(c => c.Bill).ThenInclude(c => c.User).ThenInclude(c => c.Person).SingleOrDefault();
        }

        public Bill? GetBill(int id)
        {
            return Context.Bill.Where(c => c.Id == id).Include(c => c.User).Include(c => c.Positions).SingleOrDefault();
        }


        public bool CanFinishBooking(Booking booking)
        {
            //Mock, Real Method will check Position is on Endsation before Finishing
            return true;
        }

        public bool FinishBooking(Booking booking)
        {
            if (CanFinishBooking(booking))
            {
              
                booking.Returned = true;
                booking.ReturnedTime = DateTime.UtcNow;
                booking.EndKilometers = CarService.GetMillage(booking.Car);
                CarService.LockCar(booking.Car);
                Context.SaveChanges();
                CreateBill(booking);
                return true;
            }
            return false;
        }
        
        public void CreateBill(Booking booking)
        {


            BillPositionReturn positions;
            Bill bill = new Bill(DateTime.Today, booking.User, (float)Math.Round(GetBillAmount(booking.User.Plan, booking.Carclass, booking.Starttime, booking.ReturnedTime, booking.Endtime, booking.EndKilometers - booking.StartKilometers, out positions), 2), "");
            bill.Positions = new List<BillPosition>();
            if (positions.days > 0)
                bill.Positions.Add(new BillPosition("Tage", positions.days, Math.Round(booking.User.Plan.PriceWholeDay * booking.Carclass.PriceFaktor, 2)));
            if (positions.km > 0)
                bill.Positions.Add(new BillPosition("Kilometer", positions.km, Math.Round(booking.User.Plan.PriceKm * booking.Carclass.PriceFaktor , 2)));
            if (positions.daytimeHours > 0)
                bill.Positions.Add(new BillPosition("Stunden (Tag)", positions.daytimeHours, Math.Round(booking.User.Plan.PriceHourDay * booking.Carclass.PriceFaktor, 2)));
            if (positions.nighttimeHours > 0)
                bill.Positions.Add(new BillPosition("Stunden (Nacht)", positions.nighttimeHours, Math.Round(booking.User.Plan.PriceHourNight * booking.Carclass.PriceFaktor, 2)));
            if (positions.overdueHours > 0)
                bill.Positions.Add(new BillPosition("Stunden (Überzogen)", positions.overdueHours, Math.Round(booking.User.Plan.PriceHourOverdue * booking.Carclass.PriceFaktor, 2)));


           
          

            Context.Bill.Add(bill);
            Context.Booking.Update(booking);
            booking.Bill = bill;
            Context.SaveChanges();

        }

        public double GetBillAmount(Plan plan , Carclass carclass , DateTime startTime , DateTime realEndtime , DateTime predictedEndtime , double km , out BillPositionReturn billPositions  )
        {
            billPositions = new BillPositionReturn();

            DateTime endtime = realEndtime > predictedEndtime ? realEndtime : predictedEndtime;

            int wholeDays = Math.Max(0,(endtime.Day - startTime.Day) - 1);
            billPositions.days = wholeDays;
            billPositions.km = km;



            if (endtime.Date == startTime.Date)
            {
                BillAmountDay(startTime.TimeOfDay, endtime.TimeOfDay, plan , billPositions);
            }
            else
            {         
                BillAmountDay(startTime.TimeOfDay, new TimeSpan(24 , 0 ,0), plan, billPositions);
                BillAmountDay(new TimeSpan(0, 0, 0), endtime.TimeOfDay, plan, billPositions);

            }

            billPositions.overdueHours = Math.Max(0, (realEndtime - predictedEndtime).TotalHours);

            return (float)Math.Round(billPositions.calculateAmount(carclass.PriceFaktor , plan) , 2);
          
        }

        private void BillAmountDay(TimeSpan start, TimeSpan end , Plan plan ,  BillPositionReturn positions)
        {
            TimeSpan beginMorning = new TimeSpan(5,0,0);
            TimeSpan beginNight = new TimeSpan(22, 0, 0);

            TimeSpan dayPriceStart = beginMorning > start ? beginMorning : start;
            TimeSpan dayPriceEnd = beginNight < end ? beginNight : end;

            double hDay = Math.Max((dayPriceEnd - dayPriceStart).TotalHours, 0);
            double hNight = (end - start).TotalHours - hDay;
    

            bool wholeDayIsCheaper = plan.PriceWholeDay <= hNight * plan.PriceHourNight + hDay * plan.PriceHourDay;
            if(wholeDayIsCheaper)
            {
                positions.days++;
            }
            else
            {
                positions.daytimeHours += hDay;
                positions.nighttimeHours += hNight;
            }
           
        }


        public class BillPositionReturn
        {
            public int days { get; set; }
           
            public double daytimeHours { get; set; }

            public double nighttimeHours { get; set; }

            public double overdueHours { get; set; }

            public double km { get; set; }

            public double amount { get; set; }

            

            public double calculateAmount(double factor , Plan plan) {
                amount = 0;
                amount += km * plan.PriceKm;
                amount += daytimeHours * plan.PriceHourDay;
                amount += nighttimeHours * plan.PriceHourNight;
                amount += days * plan.PriceWholeDay;
                amount += overdueHours * plan.PriceHourOverdue;
                amount *= factor;
                return amount;
            }
        }


        private struct BookingEvent
        {
            public int Id { get; private set; }
            public bool AddCar { get; private set; }
            public bool RemoveCar { get; private set; }
            public DateTime Time { get; private set; }
            public Station Station { get; private set; }
            public Carclass Carclass { get; private set; }

            public BookingEvent(int id, bool addCar, bool removeCar, DateTime time, Station station, Carclass carclass)
            {
                Id = id;
                AddCar = addCar;
                RemoveCar = removeCar;
                Time = time;
                Station = station;
                Carclass = carclass;
            }
        }
    }
}
