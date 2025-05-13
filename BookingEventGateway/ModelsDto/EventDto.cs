using Microsoft.AspNetCore.Mvc;

namespace BookingEventGateway.ModelsDto
{
    public class EventDto
    {
        public string Id { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationDto Location { get; set; } = new();
    }
    public class LocationDto
    {
        public string VenueName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
