using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BudgetTrackerWeb.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Index()
    {
      var earnings = await _context.Earnings.ToListAsync();
      var totalAmount = earnings.Sum(e => e.Amount);
      ViewBag.TotalAmount = totalAmount;
      return View(earnings);
    }

    public IActionResult Create()
    {
      return View();
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
  }
}