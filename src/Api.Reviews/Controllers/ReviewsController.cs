using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Reviews.Data;
using Api.Reviews.Models;

namespace Api.Reviews.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ReviewsDbContext _db;
    public ReviewsController(ReviewsDbContext db) => _db = db;

    // GET /api/reviews?bookId=123&page=1&pageSize=20
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetByBook(
        [FromQuery] int bookId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (bookId <= 0) return BadRequest("bookId is required");
        var q = _db.Reviews.AsNoTracking()
            .Where(r => r.BookId == bookId && r.Status == ReviewStatus.Published)
            .OrderByDescending(r => r.CreatedAt);
        var results = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(results);
    }

    // GET /api/reviews/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Review>> Get(int id)
    {
        var review = await _db.Reviews.FindAsync(id);
        return review is null ? NotFound() : Ok(review);
    }

    // POST /api/reviews
    [HttpPost]
    public async Task<ActionResult<Review>> Create([FromBody] Review review)
    {
        if (review.Stars is < 1 or > 5) return BadRequest("Stars must be 1..5");

        // TODO: pull UserId from auth claims; placeholder now:
        if (string.IsNullOrWhiteSpace(review.UserId)) review.UserId = "local-dev-user";

        review.Id = 0; review.CreatedAt = DateTime.UtcNow; review.Status = ReviewStatus.Published;
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = review.Id }, review);
    }

    // PUT /api/reviews/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Review updated)
    {
        if (id != updated.Id) return BadRequest("Id mismatch");
        if (!await _db.Reviews.AnyAsync(r => r.Id == id)) return NotFound();

        _db.Entry(updated).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/reviews/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _db.Reviews.FindAsync(id);
        if (review is null) return NotFound();

        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
