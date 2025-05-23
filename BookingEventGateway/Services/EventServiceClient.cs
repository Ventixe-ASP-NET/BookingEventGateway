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
        public async Task<List<EventDto>> GetAllAsync()
        {
            var wrapper = await _http.GetFromJsonAsync<EventListWrapper>("api/event");
            return wrapper?.Events ?? new List<EventDto>();
        }
        public async Task<EventDto?> GetByIdAsync(string id)
        {
            return await _http.GetFromJsonAsync<EventDto>($"api/Event/{id}");
        }

    }
}
