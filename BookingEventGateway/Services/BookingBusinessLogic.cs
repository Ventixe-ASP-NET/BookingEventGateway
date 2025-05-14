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
                    Category = eventDto.Category,
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

                    TicketTypes = eventDto.TicketTypes.Select(t => new EventTicketTypeDto
                    {
                        Id = t.Id,
                        TicketType = t.TicketType_,
                        Price = t.Price,
                        TotalTickets = t.TotalTickets,
                        TicketsSold = t.TicketsSold,
                        TicketsLeft = t.TicketsLeft
                    }).ToList(),

                    // Bokade biljetter hämtas senare när BookingService har stöd för det
                    BookedTickets = new() // lämnas tom så länge
                };

                result.Add(dto);
            }

            return result;
        }
    }
}
