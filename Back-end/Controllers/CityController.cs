using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [AllowAnonymous]
        public IActionResult Index(string countrySearch, string sortOrder, int page = 1)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

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

                if (country == null)
                    continue;

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

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Countries = new SelectList(countries);
            var viewModel = new CreateCityViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreateCityViewModel model)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var country = _context.Country.FirstOrDefault(c => c.Name == model.CountryName);

            if (country == null)
                return View("Error", new ErrorViewModel("Incorrect country"));


            var city = new City()
            {
                Name = model.CityName,
                CountryId = country.CountryId
            };


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(city);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - city was not created"));
                }
            }

            ViewBag.Countries = new SelectList(countries);
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var city = await _context.City.FindAsync(id);

            if (city == null)
                return View("Error", new ErrorViewModel("City not found"));

            ViewBag.ID = id;
            ViewBag.Countries = new SelectList(countries);

            var viewModel = GenerateCreateCityViewModel(id);

            if (viewModel == null)
                return View("Index", _context.City);

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(CreateCityViewModel model)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var country = _context.Country.FirstOrDefault(c => c.Name == model.CountryName);

            if (country == null)
                return View("Error", new ErrorViewModel("Incorrect country"));


            var city = _context.City.Find(model.CityId);

            if (city == null)
                return View("Error", new ErrorViewModel("City not found"));

            city.Name = model.CityName;
            city.CountryId = country.CountryId;

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - city was not edited"));
                }
            }

            ViewBag.Countries = new SelectList(countries);
            ViewBag.ID = model.CityId;

            return View(GenerateCreateCityViewModel(model.CityId));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var city = _context.City.Find(id);

            if (city == null)
                return View("Error", new ErrorViewModel("City not found"));

            var viewModel = GenerateCityViewModel(id);

            if (viewModel == null)
                return View("Index", _context.City);

            return View(viewModel);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var city = _context.City.Find(id);

            if (city == null)
                return View("Error", new ErrorViewModel("City not found"));

            try
            {
                _context.City.Remove(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel("Problem with database - city was not deleted"));
            }
        }

   
        public CityViewModel? GenerateCityViewModel(int cityId)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return null;

            var city = _context.City.Find(cityId);

            if (city == null)
                return null;

            var country = _context.Country.Find(city.CountryId);

            if (country == null)
                return null;

            var viewModel = new CityViewModel()
            {
                City = city,
                Country = country
            };

            return viewModel;
        }

        public CreateCityViewModel? GenerateCreateCityViewModel(int cityId)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return null;

            var city = _context.City.Find(cityId);

            if (city == null)
                return null;

            var country = _context.Country.Find(city.CountryId);

            if (country == null)
                return null;

            var viewModel = new CreateCityViewModel()
            {
                CityName = city.Name,
                CountryName = country.Name
            };

            return viewModel;
        }
    }
}
