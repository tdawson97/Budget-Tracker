using Microsoft.AspNetCore.Mvc;
using BudgetTrackerWeb.Models;
using Microsoft.EntityFrameworkCore;

public class CategorySummaryController : Controller
{
    private readonly BudgetContext _context;

    public CategorySummaryController(BudgetContext context)
    {
        _context = context;
    }

    public IActionResult Index(int? year, int? month)
    {
        var expensesQuery = _context.Expenses.AsQueryable();
        var earningsQuery = _context.Earnings.AsQueryable();

        if (year.HasValue && month.HasValue)
        {
            expensesQuery = expensesQuery.Where(e => e.Date.Year == year && e.Date.Month == month);
            earningsQuery = earningsQuery.Where(e => e.Date.Year == year && e.Date.Month == month);
        }

        var expenseSummary = expensesQuery
        .GroupBy(e => e.Category)
        .Select(g => new CategorySummary
        {
          Category = g.Key,
          Total = g.Sum(e => e.Amount)
        })
        .ToList();

        var earningSummary = earningsQuery
          .GroupBy(e => e.Category)
          .Select(g => new CategorySummary
          {
            Category = g.Key,
            Total = g.Sum(e => e.Amount)
          })
          .ToList();

        ViewBag.ExpenseCategories = expenseSummary.Select(e => e.Category).ToList();
        ViewBag.ExpenseTotals = expenseSummary.Select(e => e.Total).ToList();

        ViewBag.EarningCategories = earningSummary.Select(e => e.Category).ToList();
        ViewBag.EarningTotals = earningSummary.Select(e => e.Total).ToList();

        ViewBag.SelectedYear = year;
        ViewBag.SelectedMonth = month;

        return View("~/Views/CategorySummary/CategorySummary.cshtml");
    }
}
