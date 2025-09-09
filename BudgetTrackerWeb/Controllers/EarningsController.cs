using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BudgetTrackerWeb.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

namespace BudgetTrackerWeb.Controllers
{
  public class EarningsController : Controller
  {
    private readonly BudgetContext _context;
    public EarningsController(BudgetContext context)
    {
      _context = context;
    }

    // Fetch data for the earnings table
    public async Task<IActionResult> Index(string searchString, string categoryFilter)
    {
      var earnings = from e in _context.Earnings select e;

      if (!string.IsNullOrEmpty(searchString))
      {
        earnings = earnings.Where(e => e.Description.Contains(searchString));
      }

      if (!string.IsNullOrEmpty(categoryFilter))
      {
        earnings = earnings.Where(e => e.Category == categoryFilter);
      }

      ViewBag.CategoryList = new SelectList(
        await _context.Earnings.Select(e => e.Category).Distinct().ToListAsync()
      );
      
      var earningsList = await earnings.ToListAsync();
      var totalAmount = earningsList.Sum(e => e.Amount);
      ViewBag.TotalAmount = totalAmount;
      return View(earningsList);
    }

    public IActionResult Create()
    {
      return View();
    }

    public async Task<IActionResult> Edit(int id)
    {
      var earning = await _context.Earnings.FindAsync(id);
      return View(earning);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Earning earning)
    {
      if (ModelState.IsValid)
      {
        _context.Earnings.Add(earning);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(earning);
    }

    // POST: Delete id
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
      var earning = await _context.Earnings.FindAsync(id);
      if (earning != null)
      {
        _context.Earnings.Remove(earning);
        await _context.SaveChangesAsync();
      }
      return RedirectToAction(nameof(Index));
    }

    //POST: Edit id
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Earning earningEdit)
    {
      var earning = await _context.Earnings.FindAsync(earningEdit.Id);
      if (earning != null)
      {
        _context.Entry(earning).CurrentValues.SetValues(earningEdit);
        await _context.SaveChangesAsync();
      }
      return RedirectToAction(nameof(Index));
    }
  }
}