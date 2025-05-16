namespace BookingEventGateway.ModelsDto
{
    public class CategoryStatsDto
    {
        public int TotalBookings { get; set; }
        public List<CategoryItemDto> Categories { get; set; } = new();
    }

    
}
