using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;

namespace Backend.Controllers
{
    public class HotelController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IQueryable<string> cities;
        private readonly IQueryable<string> hotels;
        private readonly string[] orders;


        public HotelController(DatabaseContext context)
        {
            _context = context;

            cities = from c in _context.City
                     orderby c.Name
                     select c.Name;

            orders = new string[] { "Ascending", "Descending" };

        }

        // GET: Hotel
        public async Task<IActionResult> Index(string citySearch, string sortOrder, int page = 1)
        {
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


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hotel == null)
            {
                return NotFound();
            }

            var hotel = _context.Hotel.Find(id);
            var city = _context.City.Find(hotel.CityId);

            if (hotel == null || city == null)
            {
                return NotFound();
            }

            var viewModel = new HotelViewModel()
            {
                Hotel = hotel,
                City = city
            };

            return View(viewModel);
        }


        public IActionResult Create()
        {
            ViewBag.Cities = new SelectList(cities);
            var viewModel = new CreateHotelViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateHotelViewModel model)
        {
            var city = _context.City.First(c => c.Name == model.City);

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
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cities = new SelectList(cities);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (_context.Hotel == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }
            ViewBag.Cities = new SelectList(cities);
            ViewBag.ID = id;

            var viewModel = GenerateCreateHotelViewModel(id);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateHotelViewModel model)
        {
            var city = _context.City.First(c => c.Name == model.City);
            var hotel = _context.Hotel.Find(model.HotelId);

            hotel.Name = model.Name;
            hotel.CityId = city.CityId;
            hotel.Rate = model.Rate;
            hotel.Pool = model.Pool;
            hotel.WiFi = model.WiFi;

            if (ModelState.IsValid)
            {
                // try catch
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cities = new SelectList(cities);
            ViewBag.ID = hotel.HotelId;

            return View(model.HotelId);
        }

        // GET: Hotel/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = _context.Hotel.First(h => h.HotelId == id);

            if (hotel == null)
                return View("Index");
            else
            {
                var viewModel = GenerateHotelViewModel(id);
                return View(viewModel);
            }
        }

        // POST: Hotel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hotel == null)
            {
                return Problem("Entity set 'DatabaseContext.Hotel'  is null.");
            }
            var hotel = await _context.Hotel.FindAsync(id);

            if (hotel != null)
            {
                _context.Hotel.Remove(hotel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotelExists(int id)
        {
            return (_context.Hotel?.Any(e => e.HotelId == id)).GetValueOrDefault();
        }

        //
        public HotelViewModel GenerateHotelViewModel(int hotelId)
        {
            var hotel = _context.Hotel.Find(hotelId);
            var city = _context.City.Find(hotel.CityId);

            var viewModel = new HotelViewModel()
            {
                City = city,
                Hotel = hotel,
            };
            return viewModel;
        }

        public CreateHotelViewModel GenerateCreateHotelViewModel(int hotelId)
        {
            var hotel = _context.Hotel.Find(hotelId);
            var city = _context.City.Find(hotel.CityId);

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

