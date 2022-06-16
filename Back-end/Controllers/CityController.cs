using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;

namespace Backend.Controllers
{
    public class CityController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IQueryable<string> countries;
        private readonly string[] orders;


        public CityController(DatabaseContext context)
        {
            _context = context;

            countries = from c in _context.Country
                        orderby c.Name
                        select c.Name;

            orders = new string[] { "Ascending", "Descending" };
        }


        // GET: Cities
        public async Task<IActionResult> Index(string countrySearch, string sortOrder, int page = 1)
        {

            var cities = from c in _context.City
                         select c;


            switch (sortOrder)
            {
                case "Descending":
                    cities = cities.OrderByDescending(s => s.Name);
                    break;
                case "Ascending":
                    cities = cities.OrderBy(s => s.Name);
                    break;
            }
            var viewModels = new List<CityViewModel>();

            foreach (City city in cities)
            {
                var country = _context.Country.Find(city.CountryId);

                if (country.Name == countrySearch || string.IsNullOrEmpty(countrySearch))
                {
                    viewModels.Add(new CityViewModel()
                    {
                        City = city,
                        Country = country
                    });
                }
            }

            ViewBag.Orders = new SelectList(orders, (string.IsNullOrEmpty(sortOrder) || !orders.Contains(sortOrder)) ? "" : sortOrder);
            ViewBag.Countries = new SelectList(countries, (string.IsNullOrEmpty(countrySearch) || !countries.Contains(countrySearch)) ? "All" : countrySearch);

            viewModels = viewModels.Skip((page - 1) * 20).Take(20).ToList();

            return View(viewModels);
        }


        // GET: Cities/Create
        public IActionResult Create()
        {
            ViewBag.Countries = new SelectList(countries);
            var viewModel = new CreateCityViewModel();
            return View(viewModel);
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCityViewModel model)
        {
            var country = _context.Country.FirstOrDefault(c => c.Name == model.CountryName);
            
            if (country == null)
                return View("Error");

            var city = new City()
            {
               Name = model.CityName,
               CountryId = country.CountryId
            };

            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Countries = new SelectList(countries);
            return View(model);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.City == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            ViewBag.ID = id;
            ViewBag.Countries = new SelectList(countries);

            var viewModel = GenerateCreateCityViewModel(id);

            return View(viewModel);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateCityViewModel model)
        {
            var country = _context.Country.FirstOrDefault(c => c.Name == model.CountryName);

            if (country == null)
                return View("Error");

            var city = _context.City.Find(model.CityId);

            city.Name = model.CityName;
            city.CountryId = country.CountryId;


            if (ModelState.IsValid)
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Countries = new SelectList(countries);
            ViewBag.ID = model.CityId;
    
            return View(GenerateCreateCityViewModel(model.CityId));
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.City == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.City == null)
            {
                return Problem("Entity set 'DatabaseContext.City'  is null.");
            }
            var city = await _context.City.FindAsync(id);
            if (city != null)
            {
                _context.City.Remove(city);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return (_context.City?.Any(e => e.CityId == id)).GetValueOrDefault();
        }

        public CityViewModel GenerateCityViewModel(int cityId)
        {
            var city = _context.City.Find(cityId);
            var country = _context.Country.Find(city.CountryId);

            var viewModel = new CityViewModel()
            {
                City = city,
                Country = country
            };
            return viewModel;
        }

        public CreateCityViewModel GenerateCreateCityViewModel(int cityId)
        {
            var city = _context.City.Find(cityId);
            var country = _context.Country.Find(city.CountryId);

            var viewModel = new CreateCityViewModel()
            {
                CityName = city.Name,
                CountryName = country.Name
            };
            return viewModel;
        }
    }
}
