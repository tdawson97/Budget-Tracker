using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BudgetTrackerWeb.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
    public async Task<IActionResult> Index(string searchString, string categoryFilter)
    {
      var expenses = from e in _context.Expenses select e;

      if (!string.IsNullOrEmpty(searchString))
      {
        expenses = expenses.Where(e => e.Description.Contains(searchString));
      }

      if (!string.IsNullOrEmpty(categoryFilter))
      {
        expenses = expenses.Where(e => e.Category == categoryFilter);
      }

      ViewBag.CategoryList = new SelectList(
        await _context.Expenses.Select(e => e.Category).Distinct().ToListAsync()
      );

      var expensesList = await expenses.ToListAsync();
      var totalAmount = expensesList.Sum(e => e.Amount);
      ViewBag.TotalAmount = totalAmount;
      return View(expensesList);
    }

    public IActionResult Create()
    {
      return View();
    }

    public async Task<IActionResult> Edit(int id)
    {
      var expense = await _context.Expenses.FindAsync(id);
      return View(expense);
    }

    // Adding a new expense
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

    // POST: Delete id
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      var expense = await _context.Expenses.FindAsync(id);
      if (expense != null)
      {
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
      }
      return RedirectToAction(nameof(Index));
    }

    //POST: Edit id
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Expense expenseEdit)
    {
      var expense = await _context.Expenses.FindAsync(expenseEdit.Id);
      if (expense != null)
      {
        _context.Entry(expense).CurrentValues.SetValues(expenseEdit);
        await _context.SaveChangesAsync();
      }
      return RedirectToAction(nameof(Index));
    }
  }
}