using System.Security.Claims;
using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.ExpenseTracker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

/// <summary>
/// Controlador para la gestión de gastos de Otros / Varios.
/// </summary>
[ApiController]
[Route("api/v1/others")]
[Authorize]
public class OthersController : ControllerBase
{
    private readonly IOthersService _othersService;
    private readonly ILogger<OthersController> _logger;

    public OthersController(IOthersService othersService, ILogger<OthersController> logger)
    {
        _othersService = othersService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Others others)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        others.UserId = userId;

        var created = await _othersService.CreateOthers(others);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Others others)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        others.Id = id;
        others.UserId = userId;

        try
        {
            var updated = await _othersService.UpdateOthers(id, others, userId);
            return Ok(updated);
    }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Others expense {OthersId} not found for user {UserId}", id, userId);
            return NotFound(new { message = ex.Message });
    }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = GetUserId();

        var deleted = await _othersService.DeleteOthersById(id, userId);
        if (!deleted)
        {
            _logger.LogWarning("Others expense {OthersId} not found for deletion by user {UserId}", id, userId);
            return NotFound(new { message = $"Others expense with id '{id}' not found or does not belong to the user." });
    }

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var others = await _othersService.GetOthersById(id, GetUserId());
        if (others is null)
            return NotFound(new { message = $"Others expense with id '{id}' not found." });

        return Ok(others);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchBy = null,
        [FromQuery] string? searchString = null,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool sortAscending = true,
        [FromQuery] string? period = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        if (!string.IsNullOrWhiteSpace(period))
        {
            var now = DateTime.UtcNow;
            (startDate, endDate) = period.ToLowerInvariant() switch
            {
                "week" => (now.AddDays(-7), now),
                "month" => (now.AddMonths(-1), now),
                "3months" => (now.AddMonths(-3), now),
                _ => (startDate, endDate)
            };
    }

        var result = await _othersService.GetPaginatedOthers(
            GetUserId(),
            pageIndex,
            pageSize,
            searchBy,
            searchString,
            sortBy,
            sortAscending ? SortOrderEnum.Ascending : SortOrderEnum.Descending,
            startDate,
            endDate);

        return Ok(result);
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}