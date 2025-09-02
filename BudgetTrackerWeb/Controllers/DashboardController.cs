using BudgetTrackerWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BudgetTrackerWeb.Controllers
{
  public class DashboardController : Controller
  {
    private readonly BudgetContext _context;
    public DashboardController(BudgetContext context)
    {
      _context = context;
    }
    public async Task<IActionResult> Index(int? year, int? month)
    {
      var expensesQuery = _context.Expenses.AsQueryable();
      var earningsQuery = _context.Earnings.AsQueryable();
      
      // set up for net balance line chart data
      var allTimeExpenses = await expensesQuery.SumAsync(e => e.Amount);
      var allTimeEarnings = await earningsQuery.SumAsync(e => e.Amount);
      var allTimeNetBalance = allTimeEarnings - allTimeExpenses;

      var monthlyExpenses = await expensesQuery
        .GroupBy(e => new { e.Date.Year, e.Date.Month })
        .Select(g => new
        {
          Year = g.Key.Year,
          Month = g.Key.Month,
          Total = g.Sum(e => e.Amount)
        })
        .ToListAsync();

      var monthlyEarnings = await earningsQuery
        .GroupBy(e => new { e.Date.Year, e.Date.Month })
        .Select(g => new
        {
          Year = g.Key.Year,
          Month = g.Key.Month,
          Total = g.Sum(e => e.Amount)
        })
        .ToListAsync();


      var allMonths = monthlyExpenses.Select(e => new DateTime(e.Year, e.Month, 1))
        .Union(monthlyEarnings.Select(e => new DateTime(e.Year, e.Month, 1)))
        .Distinct()
        .OrderBy(d => d)
        .ToList();

      var netLabels = allMonths
        .Select(d => d.ToString("MMM yyyy"))
        .ToList();

      double runningTotal = 0;
      var netValues = allMonths
        .Select(d =>
        {
          var earnings = monthlyEarnings.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)?.Total ?? 0;
          var expenses = monthlyExpenses.FirstOrDefault(x => x.Year == d.Year && x.Month == d.Month)?.Total ?? 0;
          runningTotal += (double)(earnings - expenses);
          return runningTotal;
        })
        .ToList();

      ViewBag.NetLabels = netLabels;
      ViewBag.NetValues = netValues;

      // set up for filtered totals on dashboard
      if (year.HasValue && month.HasValue)
      {
        expensesQuery = expensesQuery.Where(e => e.Date.Year == year && e.Date.Month == month);
        earningsQuery = earningsQuery.Where(e => e.Date.Year == year && e.Date.Month == month);
      }

      var totalExpenses = await expensesQuery.SumAsync(e => e.Amount);
      var totalEarnings = await earningsQuery.SumAsync(e => e.Amount);
      var netBalance = totalEarnings - totalExpenses;

      ViewBag.TotalExpenses = totalExpenses;
      ViewBag.TotalEarnings = totalEarnings;
      ViewBag.NetBalance = netBalance;

      return View();
    }
  }
}