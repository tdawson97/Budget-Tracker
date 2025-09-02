using Microsoft.AspNetCore.Mvc;
using BudgetTrackerWeb.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BudgetTrackerWeb.Controllers
{
  public class ExpensesController : Controller
  {
    private readonly BudgetContext _context;
    public ExpensesController(BudgetContext context)
    {
      _context = context;
    }

    // Fetch data for the expenses table
    public async Task<IActionResult> Index()
    {
      var expenses = await _context.Expenses.ToListAsync();
      var totalAmount = expenses.Sum(e => e.Amount);
      ViewBag.TotalAmount = totalAmount;
      return View(expenses);
    }

    public IActionResult Create()
    {
      return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Expense expense)
    {
      if (ModelState.IsValid)
      {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); 
      }
      return View(expense);
    }
  }
}