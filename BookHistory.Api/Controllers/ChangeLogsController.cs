namespace BookHistory.Api.Controllers;

using BookHistory.Domain.Interfaces;
using BookHistory.Domain.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChangeLogsController : ControllerBase
{
    private readonly IBookService _service;

    public ChangeLogsController(IBookService service) => _service = service;

    /// <summary>
    /// Returns change history with pagination, filtering and ordering.
    /// Set GroupBy to "Book", "ChangeType" or "Date" to get grouped results.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ChangeLogQuery query)
    {
        if (!string.IsNullOrWhiteSpace(query.GroupBy))
        {
            var grouped = await _service.GetGroupedChangeLogsAsync(query);
            return Ok(grouped);
        }

        var paged = await _service.GetChangeLogsAsync(query);
        return Ok(paged);
    }
}