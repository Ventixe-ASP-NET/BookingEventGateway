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

                result.Add(new BookingWithEventDto
                {
                    Id = booking.Id,
                    BookingName = booking.BookingName,
                    InvoiceId = booking.InvoiceId,
                    CreatedAt = booking.CreatedAt,
                    EventId = booking.EventId,
                    EventName = eventDto?.EventName,
                    StartDate = eventDto?.StartDate,
                    EndDate = eventDto?.EndDate,
                    VenueName = eventDto?.Location?.VenueName,
                    City = eventDto?.Location?.City
                });
            }

            return result;
        }
    }
}
