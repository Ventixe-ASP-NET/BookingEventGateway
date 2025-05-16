namespace BookingEventGateway.ModelsDto
{
    public class CategoryStatsDto
    {
        public int TotalBookings { get; set; }
        public List<CategoryStat> Categories { get; set; } = new();
    }

    public class CategoryStat
    {
        public string Name { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
    }
}
