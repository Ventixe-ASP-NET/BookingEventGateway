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

        public async Task<CategoryStatsDto> GetTopCategoriesAsync()
        {
            // Hämta redan sammanslagen lista med bookings + eventdata
            var bookingsWithEvent = await GetAllBookingsWithEventsAsync();

            // Groupa bokningar efter kategori (obs! kan vara null, så vi skyddar)
            var grouped = bookingsWithEvent
                .Where(b => !string.IsNullOrEmpty(b.Category))
                .GroupBy(b => b.Category)
                .Select(g => new CategoryStat
                {
                    Name = g.Key!,
                    Count = g.Count(),
                    Percentage = 0 // sätts senare
                })
                .OrderByDescending(x => x.Count)
                .Take(4)
                .ToList();

            // Räkna totalbokningar
            int total = bookingsWithEvent.Count;

            // Lägg till procent
            foreach (var cat in grouped)
            {
                cat.Percentage = Math.Round((double)cat.Count / total * 100, 1);
            }

            // Returnera DTO
            return new CategoryStatsDto
            {
                TotalBookings = total,
                Categories = grouped
            };
        }
    }
}
