using BookingEventGateway.ModelsDto;
using Microsoft.AspNetCore.Mvc;

namespace BookingEventGateway.Services
{
    public class BookingServiceClient
    {
        private readonly HttpClient _http;

        public BookingServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<BookingModel>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<BookingModel>>("api/bookings") ?? new();
        }
    }
}
