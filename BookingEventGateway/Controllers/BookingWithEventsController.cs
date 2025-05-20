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
        public async Task<ActionResult<CategoryStatsDto>> GetTopCategories([FromQuery] string range = "week")
        {
            var stats = await _logic.GetTopCategoriesAsync(range);
            return Ok(stats);
        }

        // GET /api/bookingwithevents/paged?sort=...&order=...&page=...&pageSize=...
        [HttpGet("paged")]
        public async Task<ActionResult<PagedResponse<BookingWithEventDto>>> GetPaged(
            [FromQuery] string sort = "date",
            [FromQuery] string order = "desc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 8)
        {
            var paged = await _logic.GetPagedBookingsWithEventsAsync(sort, order, page, pageSize);
            return Ok(paged);
        }

    }
}
