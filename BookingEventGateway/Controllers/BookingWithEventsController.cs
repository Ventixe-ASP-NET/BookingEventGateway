using BookingEventGateway.ModelsDto;
using BookingEventGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingEventGateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingWithEventsController : ControllerBase
    {
        private readonly BookingBusinessLogic _logic;

        public BookingWithEventsController(BookingBusinessLogic logic)
        {
            _logic = logic;
        }

        // GET: /api/bookingwithevents
        [HttpGet]
        public async Task<ActionResult<List<BookingWithEventDto>>> Get()
        {
            var result = await _logic.GetAllBookingsWithEventsAsync();
            return Ok(result);
        }

        [HttpGet("stats/top-categories")]
        public async Task<ActionResult<CategoryStatsDto>> GetTopCategories()
        {
            var stats = await _logic.GetTopCategoriesAsync();
            return Ok(stats);
        }


    }
}
