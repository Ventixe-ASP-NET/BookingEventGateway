namespace BookingEventGateway.ModelsDto
{
    public class BookingTicketDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Guid TicketTypeId { get; set; }
        public string TicketType { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PricePerTicket { get; set; }
    }
}
