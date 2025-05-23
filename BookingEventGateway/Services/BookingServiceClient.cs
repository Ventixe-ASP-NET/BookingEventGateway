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

        public async Task<BookingModel?> GetByEvoucherCodeAsync(string code)
        {
            return await _http.GetFromJsonAsync<BookingModel>($"api/bookings/by-evoucher?code={code}");
        }
    }
}
