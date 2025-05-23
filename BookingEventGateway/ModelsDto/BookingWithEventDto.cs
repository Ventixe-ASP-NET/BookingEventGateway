using Microsoft.AspNetCore.Mvc;

namespace BookingEventGateway.ModelsDto
{
    public class BookingWithEventDto
    {
        // Booking-info
        public int Id { get; set; }
        public string EvoucherId { get; set; }
        public string BookingName { get; set; } = string.Empty;
        public string InvoiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EventId { get; set; } = string.Empty;

        // Event-info
        public string? EventName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; } // Läggs till när ni har stöd för det

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Location (hela objektet)
        public EventLocationDto? Location { get; set; }

        // Extra: Bokade biljetter (per bokning)
        public List<BookedTicketDto> BookedTickets { get; set; } = new();
    }


    //HJÄLPMODELLER
    public class EventLocationDto
    {
        public string VenueName { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }


    public class BookedTicketDto //Ej färdig, bara en skiss
    {
        public Guid TicketTypeId { get; set; }
        public string TicketType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerTicket { get; set; }
        public decimal TotalPrice => Quantity * PricePerTicket;
    }
}
