using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Expenses
        public async Task<IActionResult> Index()
        {
              return View(await _context.Expenses.ToListAsync());
        }

        public async Task<IActionResult> IndexByCategory(int? id)
        {
            return View("Index", await _context.Expenses.Where(w => w.Categories.CategoryId == id).ToListAsync());
        }

        // GET: Expenses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // GET: Expenses/Create
        public IActionResult Create(int id = 0)
        {
            // PopulateCategories();
            ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            if (id == 0)
              return View(new Expenses());
            else
                return View(_context.Expenses.Find(id));
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseId,CategoryId,Title,Description,Amount,Date")] Expenses expenses)
        {
            if (ModelState.IsValid)
            {
                float usedCategoryLimit = _context.Expenses.Where(w => w.CategoryId == expenses.CategoryId).Sum(s => s.Amount);
                float usedTotalLimit = _context.Expenses.Sum(s => s.Amount);
                if ((usedCategoryLimit + expenses.Amount) > _context.Categories.First(w=>w.CategoryId == expenses.CategoryId).Categoryexpenselimit)
                {
                    ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                    TempData["err"] = "Category limit reached."; 
                    return View(expenses);
                }
                if ((usedTotalLimit + expenses.Amount) > _context.Categories.Sum(s=>s.Categoryexpenselimit))
                {
                    ViewData["Categories"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                    TempData["err"] = "Total limit reached."; 
                    return View(expenses);
                }
                _context.Add(expenses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expenses);
        }

        // GET: Expenses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses == null)
            {
                return NotFound();
            }
            return View(expenses);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseId,CategoryId,Title,Description,Amount,Date")] Expenses expenses)
        {
            if (id != expenses.ExpenseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpensesExists(expenses.ExpenseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(expenses);
        }

        // GET: Expenses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Expenses == null)
            {
                return NotFound();
            }

            var expenses = await _context.Expenses
                .FirstOrDefaultAsync(m => m.ExpenseId == id);
            if (expenses == null)
            {
                return NotFound();
            }

            return View(expenses);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Expenses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Expenses'  is null.");
            }
            var expenses = await _context.Expenses.FindAsync(id);
            if (expenses != null)
            {
                _context.Expenses.Remove(expenses);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpensesExists(int id)
        {
          return _context.Expenses.Any(e => e.ExpenseId == id);
        }

    }
}
