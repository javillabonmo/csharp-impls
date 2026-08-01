using System.Security.Claims;
using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.ExpenseTracker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

/// <summary>
/// Controlador para la gestión de gastos de Ocio / Entretenimiento.
/// </summary>
[ApiController]
[Route("api/v1/leisure")]
[Authorize]
public class LeisureController : ControllerBase
{
    private readonly ILeisureService _leisureService;
    private readonly ILogger<LeisureController> _logger;

    public LeisureController(ILeisureService leisureService, ILogger<LeisureController> logger)
    {
        _leisureService = leisureService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Leisure leisure)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        leisure.UserId = userId;

        var created = await _leisureService.CreateLeisure(leisure);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Leisure leisure)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        leisure.Id = id;
        leisure.UserId = userId;

        try
        {
            var updated = await _leisureService.UpdateLeisure(id, leisure, userId);
            return Ok(updated);
    }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Leisure expense {LeisureId} not found for user {UserId}", id, userId);
            return NotFound(new { message = ex.Message });
    }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = GetUserId();

        var deleted = await _leisureService.DeleteLeisureById(id, userId);
        if (!deleted)
        {
            _logger.LogWarning("Leisure expense {LeisureId} not found for deletion by user {UserId}", id, userId);
            return NotFound(new { message = $"Leisure with id '{id}' not found or does not belong to the user." });
    }

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var leisure = await _leisureService.GetLeisureById(id, GetUserId());
        if (leisure is null)
            return NotFound(new { message = $"Leisure with id '{id}' not found." });

        return Ok(leisure);
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

        var result = await _leisureService.GetPaginatedLeisures(
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