using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
          private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
         {
             _context = context;
         }
        public IActionResult Index()
        {

            //TotalExpensesofboth
              int TotalExpense = _context.Expenses
               .Sum(j => j.Amount);
             ViewBag.TotalExpense = TotalExpense.ToString("C0");
            List<Categories> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            List<Expenses> expenses = _context.Expenses.Include(x => x.Categories).ToList();
            ViewBag.Expenses = expenses;
            return View();         
        }

        
    }
}
