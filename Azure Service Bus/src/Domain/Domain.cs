namespace Domain
{
    //Model class for messaging
    public class Booking
    {
        public class AirBooking
        {
            public string? To { get; set; }
            public string? From { get; set; }
            public DateTime FlightDate { get; set; }
        }

        public class HotelBooking
        {
            public string? City { get; set; }
            public DateTime CheckinDate { get; set; }
            public DateTime LeaveDate { get; set; }
        }

        public AirBooking[]? AirBookings { get; set; }
        public HotelBooking[]? HotelBookings { get; set; }

        public override string ToString()
        {
            var air = AirBookings == null ? ["No flights"] : AirBookings.Select(x => $"Flight: {x.To}=>{x.From} {x.FlightDate.Date}");
            var hotel = HotelBookings == null ? ["No hotels"] : HotelBookings.Select(x => $"Hotel: {x.City} {x.CheckinDate.Date}").ToArray();
            return
                string.Join("\r\n", air.Union(hotel));

        }
    }
}
