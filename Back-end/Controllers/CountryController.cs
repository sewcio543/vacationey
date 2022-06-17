using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Models.DbModels;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Backend.Controllers
{
    public class CountryController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly string[] orders;


        public CountryController(DatabaseContext context)
        {
            _context = context;
            orders = new string[] { "Ascending", "Descending" };

        }
        /// <summary>
        /// Hello
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Index(string sortOrder, int page = 1)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var countries = from c in _context.Country
                         select c;

            switch (sortOrder)
            {
                case "Descending":
                    countries = countries.OrderByDescending(s => s.Name);
                    break;
                case "Ascending":
                    countries = countries.OrderBy(s => s.Name);
                    break;
            }

            ViewBag.Orders = new SelectList(orders, (string.IsNullOrEmpty(sortOrder) || !orders.Contains(sortOrder)) ? "" : sortOrder);

            var countries_ = countries.Skip((page - 1) * 20).Take(20).ToList();

            return View(countries_);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Country country)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - country was not created"));
                }
            }

            return View(country);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));


            var country = await _context.Country.FindAsync(id);

            if (country == null)
                return View("Error", new ErrorViewModel("Country not found"));

            ViewBag.ID = id;
            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(Country country)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var country_ = _context.Country.Find(country.CountryId);

            if (country_ == null)
                return View("Error", new ErrorViewModel("Country not found"));

            country_.Name = country.Name;

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - country was not edited"));
                }
            }
            return View(country);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));


            var country = await _context.Country.FirstOrDefaultAsync(m => m.CountryId == id);

            if (country == null)
                return View("Error", new ErrorViewModel("Country not found"));


            return View(country);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var country = await _context.Country.FindAsync(id);

            if (country == null)
                return View("Error", new ErrorViewModel("Country not found"));


            try
            {
                _context.Country.Remove(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel("Problem with database - country was not deleted"));
            }
        }

    }
}
