using System.Security.Claims;
using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.ExpenseTracker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

/// <summary>
/// Controlador para la gestión de gastos de Servicios / Utilidades.
/// </summary>
[ApiController]
[Route("api/v1/utilities")]
[Authorize]
public class UtilitiesController : ControllerBase
{
    private readonly IUtilitiesService _utilitiesService;
    private readonly ILogger<UtilitiesController> _logger;

    public UtilitiesController(IUtilitiesService utilitiesService, ILogger<UtilitiesController> logger)
    {
        _utilitiesService = utilitiesService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Utilities utilities)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        utilities.UserId = userId;

        var created = await _utilitiesService.CreateUtilities(utilities);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Utilities utilities)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        utilities.Id = id;
        utilities.UserId = userId;

        try
        {
            var updated = await _utilitiesService.UpdateUtilities(id, utilities, userId);
            return Ok(updated);
    }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Utilities expense {UtilitiesId} not found for user {UserId}", id, userId);
            return NotFound(new { message = ex.Message });
    }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = GetUserId();

        var deleted = await _utilitiesService.DeleteUtilitiesById(id, userId);
        if (!deleted)
        {
            _logger.LogWarning("Utilities expense {UtilitiesId} not found for deletion by user {UserId}", id, userId);
            return NotFound(new { message = $"Utilities expense with id '{id}' not found or does not belong to the user." });
    }

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var utilities = await _utilitiesService.GetUtilitiesById(id, GetUserId());
        if (utilities is null)
            return NotFound(new { message = $"Utilities expense with id '{id}' not found." });

        return Ok(utilities);
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

        var result = await _utilitiesService.GetPaginatedUtilities(
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