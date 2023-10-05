using CocukSubeProject.Entities;
using CocukSubeProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CocukSubeProject.Controllers
{
    public class ChartController : Controller
    {
        private readonly DatabaseContext _databaseContext;

        public ChartController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IActionResult> Index()
        {
            var suspects = await _databaseContext.Suspects.ToListAsync();
            return View(suspects);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(string district, bool isUnder18, DateTime? birthDate)
        {
            var query = _databaseContext.Suspects.AsQueryable();

            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(s => s.District == district);
            }

            if (isUnder18)
            {
                DateTime eighteenYearsAgo = DateTime.Now.AddYears(-18);
                query = query.Where(s => s.DateOfBirth > eighteenYearsAgo);
            }

            if (birthDate.HasValue)
            {
                query = query.Where(s => s.DateOfBirth == birthDate);
            }

            var filteredSuspects = await query.ToListAsync();

            return PartialView("_FilteredSuspectsPartial", filteredSuspects);
        }
    }
}
