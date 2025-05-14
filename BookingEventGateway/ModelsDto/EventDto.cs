using Microsoft.AspNetCore.Mvc;

namespace BookingEventGateway.ModelsDto
{
    public class EventDto
    {
        public string Id { get; set; } = string.Empty;
        public string EventName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CategoryDto? Category { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public LocationDto Location { get; set; } = new();
        public List<TicketTypeDto> TicketTypes { get; set; } = new();
        public int TotalTickets { get; set; }
    }

    public class LocationDto
    {
        public string VenueName { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class TicketTypeDto
    {
        public Guid Id { get; set; }
        public string TicketType_ { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int TotalTickets { get; set; }
        public int TicketsSold { get; set; }
        public int TicketsLeft { get; set; }
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
