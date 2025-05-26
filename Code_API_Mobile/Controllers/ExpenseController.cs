using Code_API_Mobile.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Code_API_Mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly MobileDbContext _context;

        public ExpenseController(MobileDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetAll()
        {
            return await _context.Expenses
                .Include(e => e.Category)
                .Include(e => e.User)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetById(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return NotFound();
            return expense;
        }

        [HttpPost]
        public async Task<ActionResult<Expense>> Create([FromBody] Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Expense updated)
        {
            if (id != updated.Id) return BadRequest();

            _context.Entry(updated).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return NotFound();

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Tìm kiếm theo UserId + CategoryId
        [HttpGet("search/user/{userId}/category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetByUserAndCategory(int userId, int categoryId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId && e.CategoryId == categoryId)
                .ToListAsync();
        }

        // ✅ Tìm kiếm theo UserId + Ngày
        [HttpGet("search/user/{userId}/date/{date}")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetByUserAndDate(int userId, DateTime date)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId && e.Date.Date == date.Date)
                .ToListAsync();
        }
    }

}
