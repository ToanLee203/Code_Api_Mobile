using Code_API_Mobile.ModelDTO;
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
        public async Task<ActionResult<Expense>> Create([FromBody] ExpenseRequest req)
        {
            var expense = new Expense
            {
                Amount = req.Amount,
                Description = req.Description,
                Date = req.Date,
                CategoryId = req.CategoryId,
                UserId = req.UserId
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseRequest req)
        {
            var existingExpense = await _context.Expenses.FindAsync(id);
            if (existingExpense == null) return NotFound();

            existingExpense.Amount = req.Amount;
            existingExpense.Description = req.Description;
            existingExpense.Date = req.Date;
            existingExpense.CategoryId = req.CategoryId;
            existingExpense.UserId = req.UserId;

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

        // ✅ API: Tìm kiếm theo userId + ngày + categoryId
        [HttpGet("search/user/{userId}/date/{date}/category/{categoryId}")]
        public async Task<ActionResult<List<Expense>>> GetByUserDateAndCategory(int userId, DateTime date, int categoryId)
        {
            var result = await _context.Expenses
                .Where(e => e.UserId == userId
                            && e.CategoryId == categoryId
                            && e.Date.Date == date.Date)
                .ToListAsync();

            return result;
        }


        [HttpGet("search/user/{userId}/date/{date}")]
        public async Task<ActionResult<List<Expense>>> GetByUserAndDate(int userId, DateTime date)
        {
            var result = await _context.Expenses
                .Where(e => e.UserId == userId && e.Date.Date == date.Date)
                .ToListAsync();

            return result;
        }

        [HttpGet("search/user/{userId}/from/{fromDate}/to/{toDate}")]
        public async Task<ActionResult<List<Expense>>> GetByUserAndDateRange(int userId, DateTime fromDate, DateTime toDate)
        {
            var result = await _context.Expenses
                .Where(e => e.UserId == userId &&
                            e.Date.Date >= fromDate.Date &&
                            e.Date.Date <= toDate.Date)
                .Include(e => e.Category)
                .ToListAsync();

            return result;
        }


        [HttpGet("search/user/{userId}/from/{fromDate}/to/{toDate}/category/{categoryId}")]
        public async Task<ActionResult<List<Expense>>> GetByUserDateRangeAndCategory(int userId, DateTime fromDate, DateTime toDate, int categoryId)
        {
            var result = await _context.Expenses
                .Where(e => e.UserId == userId &&
                            e.CategoryId == categoryId &&
                            e.Date.Date >= fromDate.Date &&
                            e.Date.Date <= toDate.Date)
                .Include(e => e.Category)
                .ToListAsync();

            return result;
        }


    }

}
