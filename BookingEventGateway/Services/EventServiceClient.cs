using BookingEventGateway.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace BookingEventGateway.Services
{
    public class EventServiceClient
    {
        private readonly HttpClient _http;

        public EventServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<EventDto?> GetEventByIdAsync(string id)
        {
            try
            {
                var response = await _http.GetFromJsonAsync<EventDto>($"api/Event/{id}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EventServiceClient error for ID {id}: {ex.Message}");
                return null;
            }
        }
    }
}
