using Microsoft.AspNetCore.Mvc;
using BookingEventGateway.ModelsDto;
using BookingEventGateway.Services;

namespace BookingEventGateway.Services
{
    public class BookingBusinessLogic
    {
        private readonly BookingServiceClient _bookingClient;
        private readonly EventServiceClient _eventClient;
        public BookingBusinessLogic(BookingServiceClient bookingClient, EventServiceClient eventClient)
        {
            _bookingClient = bookingClient;
            _eventClient = eventClient;
        }

        public async Task<List<BookingWithEventDto>> GetAllBookingsWithEventsAsync()
        {
            var bookings = await _bookingClient.GetAllAsync();
            var result = new List<BookingWithEventDto>();

            foreach (var booking in bookings)
            {
                var eventDto = await _eventClient.GetEventByIdAsync(booking.EventId);
                if (eventDto == null)
                {
                    Console.WriteLine($"Event not found for booking {booking.Id}");
                    continue;
                }

                var dto = new BookingWithEventDto
                {
                    // Booking info
                    Id = booking.Id,
                    BookingName = booking.BookingName,//Denna måste ändras sen till ProfileServiceId
                    InvoiceId = booking.InvoiceId,
                    CreatedAt = booking.CreatedAt,
                    EventId = booking.EventId,

                    // Event info
                    EventName = eventDto.EventName,
                    Description = eventDto.Description,
                    Category = eventDto.Category?.CategoryName,
                    StartDate = eventDto.StartDate,
                    EndDate = eventDto.EndDate,

                    Location = new EventLocationDto
                    {
                        VenueName = eventDto.Location.VenueName,
                        StreetAddress = eventDto.Location.StreetAddress,
                        City = eventDto.Location.City,
                        PostalCode = eventDto.Location.PostalCode,
                        Country = eventDto.Location.Country
                    },

                    BookedTickets = booking.Tickets.Select(t => new BookedTicketDto
                    {
                        TicketTypeId = t.TicketTypeId,
                        TicketType = t.TicketType,
                        Quantity = t.Quantity,
                        PricePerTicket = t.PricePerTicket
                    }).ToList()
                };

                result.Add(dto);
            }

            return result;
        }

        public async Task<CategoryStatsDto> GetTopCategoriesAsync(string range)
        {
            var all = await GetAllBookingsWithEventsAsync();

            var now = DateTime.UtcNow;
            IEnumerable<BookingWithEventDto> filtered = range.ToLower() switch
            {
                "today" => all.Where(b => b.CreatedAt.Date == now.Date),
                "week" => all.Where(b => b.CreatedAt >= now.AddDays(-7)),
                "month" => all.Where(b => b.CreatedAt >= now.AddMonths(-1)),
                _ => all
            };

            var grouped = filtered
                .GroupBy(b => b.Category ?? "Uncategorized")
                .Select(g => new CategoryItemDto
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(c => c.Count)
                .Take(4)
                .ToList();

            var total = grouped.Sum(c => c.Count);
            foreach (var c in grouped)
                c.Percentage = total > 0 ? Math.Round((double)c.Count / total * 100, 1) : 0;

            return new CategoryStatsDto
            {
                TotalBookings = total,
                Categories = grouped
            };
        }
    }
}
