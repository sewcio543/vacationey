using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;
using Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    public class HotelController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IQueryable<string> cities;
        private readonly string[] orders;


        public HotelController(DatabaseContext context)
        {
            _context = context;

            cities = from c in _context.City
                     orderby c.Name
                     select c.Name;

            orders = new string[] { "Ascending", "Descending" };
        }

        [AllowAnonymous]
        public IActionResult Index(string citySearch, string sortOrder, int page = 1)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotels = from h in _context.Hotel
                         select h;

            switch (sortOrder)
            {
                case "Descending":
                    hotels = hotels.OrderByDescending(s => s.Rate);
                    break;
                case "Ascending":
                    hotels = hotels.OrderBy(s => s.Rate);
                    break;
            }
            var viewModels = new List<HotelViewModel>();

            foreach (Hotel hotel in hotels)
            {
                var city = _context.City.Find(hotel.CityId);

                if (city == null)
                    continue;

                if (city.Name == citySearch || string.IsNullOrEmpty(citySearch))
                {
                    viewModels.Add(new HotelViewModel()
                    {
                        Hotel = hotel,
                        City = city
                    });
                }
            }

            ViewBag.Orders = new SelectList(orders, (string.IsNullOrEmpty(sortOrder) || !orders.Contains(sortOrder)) ? "" : sortOrder);
            ViewBag.Cities = new SelectList(cities, (string.IsNullOrEmpty(citySearch) || !cities.Contains(citySearch)) ? "All" : citySearch);

            viewModels = viewModels.Skip((page - 1) * 10).Take(10).ToList();

            return View(viewModels);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotel = _context.Hotel.Find(id);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            var city = _context.City.Find(hotel.CityId);

            if (city == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            var viewModel = new HotelViewModel()
            {
                Hotel = hotel,
                City = city
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            ViewBag.Cities = new SelectList(cities);
            var viewModel = new CreateHotelViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreateHotelViewModel model)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var city = _context.City.FirstOrDefault(c => c.Name == model.City);

            if (city == null)
                return View("Error", new ErrorViewModel("Incorrect city"));

            var hotel = new Hotel()
            {
                Name = model.Name,
                CityId = city.CityId,
                WiFi = model.WiFi,
                Pool = model.Pool,
                Rate = model.Rate
            };

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(hotel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - hotel was not created"));
                }
            }

            ViewBag.Cities = new SelectList(cities);
            return View(model);
        }


        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotel = await _context.Hotel.FindAsync(id);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Hotel not found"));


            ViewBag.Cities = new SelectList(cities);
            ViewBag.ID = id;

            var viewModel = GenerateCreateHotelViewModel(id);

            if (viewModel == null)
                return View("Index", _context.Hotel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(CreateHotelViewModel model)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var city = _context.City.FirstOrDefault(c => c.Name == model.City);

            if (city == null)
                return View("Error", new ErrorViewModel("Incorrect city"));

            var hotel = _context.Hotel.Find(model.HotelId);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Incorrect hotel"));

            hotel.Name = model.Name;
            hotel.CityId = city.CityId;
            hotel.Rate = model.Rate;
            hotel.Pool = model.Pool;
            hotel.WiFi = model.WiFi;

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View("Error", new ErrorViewModel("Problem with database - hotel was not edited"));
                }
            }

            ViewBag.Cities = new SelectList(cities);
            ViewBag.ID = hotel.HotelId;

            return View(model.HotelId);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotel = _context.Hotel.Find(id);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            var viewModel = GenerateHotelViewModel(id);

            if (viewModel == null)
                return View("Index", _context.Hotel);

            return View(viewModel);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return View("Error", new ErrorViewModel("Problem with database"));

            var hotel = _context.Hotel.Find(id);

            if (hotel == null)
                return View("Error", new ErrorViewModel("Hotel not found"));

            try
            {
                _context.Hotel.Remove(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new ErrorViewModel("Problem with database - hotel was not deleted"));
            }
        }


        public HotelViewModel? GenerateHotelViewModel(int hotelId)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return null;

            var hotel = _context.Hotel.Find(hotelId);

            if (hotel == null)
                return null;

            var city = _context.City.Find(hotel.CityId);

            if (city == null)
                return null;

            var viewModel = new HotelViewModel()
            {
                City = city,
                Hotel = hotel,
            };

            return viewModel;
        }

        public CreateHotelViewModel? GenerateCreateHotelViewModel(int hotelId)
        {
            if (_context.Hotel == null || _context.Offer == null || _context.Country == null || _context.City == null)
                return null;

            var hotel = _context.Hotel.Find(hotelId);

            if (hotel == null)
                return null;

            var city = _context.City.Find(hotel.CityId);

            if (city == null)
                return null;

            var viewModel = new CreateHotelViewModel()
            {
                HotelId = hotelId,
                Name = hotel.Name,
                City = city.Name,
                Rate = hotel.Rate,
                WiFi = hotel.WiFi,
                Pool = hotel.Pool
            };

            return viewModel;
        }
    }
}

