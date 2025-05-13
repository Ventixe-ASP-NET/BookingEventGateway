using BookingEventGateway.ModelsDto;
using BookingEventGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingEventGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingWithEventsController : Controller
    {
        private readonly BookingServiceClient _bookingClient;

        public BookingWithEventsController(BookingServiceClient bookingClient)
        {
            _bookingClient = bookingClient;
        }







        //Denna Get är bara för att testa för att hämta bokningar
        [HttpGet]
        public async Task<ActionResult<List<BookingModel>>> Get()
        {
            var result = await _bookingClient.GetAllAsync();
            return Ok(result);
        }
    }
}
