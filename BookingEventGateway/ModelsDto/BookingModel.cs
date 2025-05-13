using Microsoft.AspNetCore.Mvc;

namespace BookingEventGateway.ModelsDto
{
    public class BookingModel
    {
        public int Id { get; set; }
        public string BookingName { get; set; } = string.Empty;
        public int InvoiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EventId { get; set; } = string.Empty;
    }
}
