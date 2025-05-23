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
            var events = await _eventClient.GetAllAsync();
            var dict = events.ToDictionary(e => e.Id);

            var result = bookings.Select(booking =>
            {
                dict.TryGetValue(booking.EventId, out var ev);

                return new BookingWithEventDto
                {
                    Id = booking.Id,
                    EvoucherId = booking.EvoucherId,
                    BookingName = booking.BookingName,
                    InvoiceId = booking.InvoiceId,
                    CreatedAt = booking.CreatedAt,
                    EventId = booking.EventId,

                    // Event-info (ev kan vara null om inte hittad)
                    EventName = ev?.EventName,
                    Description = ev?.Description,
                    Category = ev?.Category?.CategoryName,
                    StartDate = ev?.StartDate,
                    EndDate = ev?.EndDate,
                    Location = ev != null
                                    ? new EventLocationDto
                                    {
                                        VenueName = ev.Location.VenueName,
                                        StreetAddress = ev.Location.StreetAddress,
                                        City = ev.Location.City,
                                        PostalCode = ev.Location.PostalCode,
                                        Country = ev.Location.Country
                                    }
                                    : null,

                    BookedTickets = booking.Tickets
                                           .Select(t => new BookedTicketDto
                                           {
                                               TicketTypeId = t.TicketTypeId,
                                               TicketType = t.TicketType,
                                               Quantity = t.Quantity,
                                               PricePerTicket = t.PricePerTicket
                                           })
                                           .ToList()
                };
            }).ToList();

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
        public async Task<PagedResponse<BookingWithEventDto>> GetPagedBookingsWithEventsAsync(
        string sort, string order, int page, int pageSize)
        {
            var all = await GetAllBookingsWithEventsAsync();

            IOrderedEnumerable<BookingWithEventDto> sorted = (sort.ToLower(), order.ToLower()) switch
            {
                ("invoice", "asc") => all.OrderBy(b => b.InvoiceId),
                ("invoice", "desc") => all.OrderByDescending(b => b.InvoiceId),

                ("date", "asc") => all.OrderBy(b => b.CreatedAt),
                ("date", "desc") => all.OrderByDescending(b => b.CreatedAt),

                ("name", "asc") => all.OrderBy(b => b.BookingName),
                ("name", "desc") => all.OrderByDescending(b => b.BookingName),

                ("event", "asc") => all.OrderBy(b => b.EventName),
                ("event", "desc") => all.OrderByDescending(b => b.EventName),

                ("category", "asc") => all.OrderBy(b => b.Category),
                ("category", "desc") => all.OrderByDescending(b => b.Category),

                ("price", "asc") => all.OrderBy(b => b.BookedTickets.Min(t => t.PricePerTicket)),
                ("price", "desc") => all.OrderByDescending(b => b.BookedTickets.Max(t => t.PricePerTicket)),

                ("qty", "asc") => all.OrderBy(b => b.BookedTickets.Sum(t => t.Quantity)),
                ("qty", "desc") => all.OrderByDescending(b => b.BookedTickets.Sum(t => t.Quantity)),

                ("amount", "asc") => all.OrderBy(b => b.BookedTickets.Sum(t => t.TotalPrice)),
                ("amount", "desc") => all.OrderByDescending(b => b.BookedTickets.Sum(t => t.TotalPrice)),

                ("status", "asc") => all.OrderBy(b => "Confirmed"),
                ("status", "desc") => all.OrderByDescending(b => "Confirmed"),

                ("evoucher", "asc") => all.OrderBy(b => b.EvoucherId),
                ("evoucher", "desc") => all.OrderByDescending(b => b.EvoucherId),

                _ => all.OrderByDescending(b => b.CreatedAt)
            };

            var total = sorted.Count();
            var items = sorted
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResponse<BookingWithEventDto>
            {
                Items = items,
                TotalCount = total
            };
        }

        public async Task<BookingWithEventDto?> GetByEvoucherCodeAsync(string code)
        {
            var booking = await _bookingClient.GetByEvoucherCodeAsync(code);
            if (booking == null) return null;

            var ev = await _eventClient.GetByIdAsync(booking.EventId);

            return new BookingWithEventDto
            {
                Id = booking.Id,
                BookingName = booking.BookingName,
                InvoiceId = booking.InvoiceId,
                CreatedAt = booking.CreatedAt,
                EventId = booking.EventId,

                EventName = ev?.EventName,
                Description = ev?.Description,
                Category = ev?.Category?.CategoryName,
                StartDate = ev?.StartDate,
                EndDate = ev?.EndDate,
                Location = ev != null
                    ? new EventLocationDto
                    {
                        VenueName = ev.Location.VenueName,
                        StreetAddress = ev.Location.StreetAddress,
                        City = ev.Location.City,
                        PostalCode = ev.Location.PostalCode,
                        Country = ev.Location.Country
                    }
                    : null,

                BookedTickets = booking.Tickets.Select(t => new BookedTicketDto
                {
                    TicketTypeId = t.TicketTypeId,
                    TicketType = t.TicketType,
                    Quantity = t.Quantity,
                    PricePerTicket = t.PricePerTicket
                }).ToList()
            };
        }
    }
}

