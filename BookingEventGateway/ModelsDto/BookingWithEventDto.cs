using Microsoft.AspNetCore.Mvc;

namespace BookingEventGateway.ModelsDto
{
    public class BookingWithEventDto
    {
        // Booking-info
        public int Id { get; set; }
        public string BookingName { get; set; } = string.Empty;
        public int InvoiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EventId { get; set; } = string.Empty;

        // Event-info
        public string? EventName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? VenueName { get; set; }
        public string? City { get; set; }
    }
}
