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
    public class HotelController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IQueryable<string> cities;
        private readonly IQueryable<string> hotels;

        public HotelController(DatabaseContext context)
        {
            _context = context;
            cities = from c in _context.City
                     group c by c.Name
                     into distinct
                     orderby distinct.First().Name
                     select distinct.First().Name;

        }

        // GET: Hotel
        public async Task<IActionResult> Index(string citySearch, string sortOrder, int page = 1)
        {

            IQueryable<string> cityQuery = from c in _context.City
                                           where c.Name != citySearch
                                           orderby c.Name
                                           select c.Name;

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

            ViewBag.Order = string.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            ViewBag.City = string.IsNullOrEmpty(citySearch) ? "All" : citySearch;
            ViewBag.Cities = new SelectList(cityQuery.Distinct().ToList());

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
            var rates = from h in _context.Hotel
                        group h by h.Rate
                        into distinct
                        orderby distinct.First().Rate
                        select distinct.First().Rate;

            ViewBag.Cities = new SelectList(cities);
            ViewBag.Rates = new SelectList(rates, 3);
            var viewModel = new CreateHotelViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HotelId,Name,Rate,CityId,Pool,WiFi")] Hotel hotel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "Name", hotel.CityId);
            return View(hotel);
        }

        // GET: Hotel/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Hotel == null)
        //    {
        //        return NotFound();
        //    }

        //    var hotel = await _context.Hotel.FindAsync(id);
        //    if (hotel == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CityId"] = new SelectList(_context.City, "CityId", "Name", hotel.CityId);
        //    return View(hotel);
        //}

        // POST: Hotel/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HotelId,Name,Rate,CityId,Pool,WiFi")] Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.HotelId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "Name", hotel.CityId);
            return View(hotel);
        }

        // GET: Hotel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hotel == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .Include(h => h.City)
                .FirstOrDefaultAsync(m => m.HotelId == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
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
    }
}
