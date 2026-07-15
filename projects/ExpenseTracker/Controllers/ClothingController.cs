using ExpenseTracker.Enums;
using ExpenseTracker.Models.ExpenseTracker;
using ExpenseTracker.Services.ExpenseTracker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers;

[ApiController]
[Route("api/v1/clothing")]
[Authorize]
public class ClothingController : ControllerBase
{
    private readonly IClothingService _clothingService;

    public ClothingController(IClothingService clothingService)
    {
        _clothingService = clothingService;
    }

    private Guid GetUserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Clothing clothing)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        clothing.UserId = GetUserId();

        var created = await _clothingService.CreateClothing(clothing);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Clothing clothing)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            clothing.Id = id;
            clothing.UserId = GetUserId();
            var updated = await _clothingService.UpdateClothing(id, clothing, GetUserId());
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _clothingService.DeleteClothingById(id, GetUserId());
        if (!deleted)
            return NotFound(new { message = $"Clothing with id '{id}' not found or does not belong to the user." });

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var clothing = await _clothingService.GetClothingById(id, GetUserId());
        if (clothing is null)
            return NotFound(new { message = $"Clothing with id '{id}' not found." });

        return Ok(clothing);
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

        var result = await _clothingService.GetPaginatedClothings(
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
}
