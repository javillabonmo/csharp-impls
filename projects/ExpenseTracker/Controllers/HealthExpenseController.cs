using System.Security.Claims;
using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.ExpenseTracker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

/// <summary>
/// Controlador para la gestión de gastos de Salud.
/// </summary>
[ApiController]
[Route("api/v1/health-expenses")]
[Authorize]
public class HealthExpenseController : ControllerBase
{
    private readonly IHealthExpenseService _healthExpenseService;
    private readonly ILogger<HealthExpenseController> _logger;

    public HealthExpenseController(IHealthExpenseService healthExpenseService, ILogger<HealthExpenseController> logger)
    {
        _healthExpenseService = healthExpenseService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Health healthExpense)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        healthExpense.UserId = userId;

        var created = await _healthExpenseService.CreateHealthExpense(healthExpense);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Health healthExpense)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetUserId();
        healthExpense.Id = id;
        healthExpense.UserId = userId;

        try
        {
            var updated = await _healthExpenseService.UpdateHealthExpense(id, healthExpense, userId);
            return Ok(updated);
    }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning("Health expense {HealthExpenseId} not found for user {UserId}", id, userId);
            return NotFound(new { message = ex.Message });
    }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = GetUserId();

        var deleted = await _healthExpenseService.DeleteHealthExpenseById(id, userId);
        if (!deleted)
        {
            _logger.LogWarning("Health expense {HealthExpenseId} not found for deletion by user {UserId}", id, userId);
            return NotFound(new { message = $"Health expense with id '{id}' not found or does not belong to the user." });
    }

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var healthExpense = await _healthExpenseService.GetHealthExpenseById(id, GetUserId());
        if (healthExpense is null)
            return NotFound(new { message = $"Health expense with id '{id}' not found." });

        return Ok(healthExpense);
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

        var result = await _healthExpenseService.GetPaginatedHealthExpenses(
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