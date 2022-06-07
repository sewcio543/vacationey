using Backend.Models;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Backend.Controllers
{
    public class OfferController : Controller
    {
        private readonly DatabaseContext _context;

        public OfferController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string countrySearch, string sortOrder)
        {
            IQueryable<string> countryQuery = from c in _context.Country
                                              select c.Name;

            var offers = from o in _context.Offer
                         select o;

            switch (sortOrder)
            {
                case "desc":
                    offers = offers.OrderByDescending(s => s.Price);
                    break;
                case "asc":
                    offers = offers.OrderBy(s => s.Price);
                    break;
            }

            List<OfferViewModel> offersViewModels = new List<OfferViewModel>();

            foreach (var offer in offers)
            {
                var hotel = _context.Hotel.Where(Hotel => Hotel.HotelId == offer.HotelId).First();
                var city = _context.City.Where(City => City.CityId == hotel.CityId).First();
                var country = _context.Country.Where(c => c.CountryId == city.CountryId).First();

                if (!string.IsNullOrEmpty(countrySearch))
                {
                    if (country.Name == countrySearch)
                    {
                        offersViewModels.Add(new OfferViewModel()
                        {
                            Country = country,
                            Offer = offer,
                            Hotel = hotel,
                            City = city
                        });

                    }
                }
                else
                {
                    offersViewModels.Add(new OfferViewModel()
                    {
                        Country = country,
                        Offer = offer,
                        Hotel = hotel,
                        City = city
                    });
                }

            }

            if (!string.IsNullOrEmpty(countrySearch))
            {
                ViewBag.Option = countrySearch;
            }
            ViewBag.Option = "XD";// (string.IsNullOrEmpty(countrySearch) || countryQuery.Distinct().ToList().Contains(countrySearch)) ? "All" : countrySearch;

            ViewBag.Countries = new SelectList(await countryQuery.Distinct().ToListAsync());


            return View(offersViewModels);
        }

        public IActionResult Search(string? countrySearch, string sortOrder)
        {
            IQueryable<string> countryQuery = from c in _context.Country
                                              select c.Name;

            var offers = from o in _context.Offer
                         select o;


            switch (sortOrder)
            {
                case "desc":
                    offers = offers.OrderByDescending(s => s.Price);
                    break;
                case "asc":
                    offers = offers.OrderBy(s => s.Price);
                    break;
            }

            List<OfferViewModel> offersViewModels = new List<OfferViewModel>();

            foreach (var offer in offers)
            {
                var hotel = _context.Hotel.Where(Hotel => Hotel.HotelId == offer.HotelId).First();
                var city = _context.City.Where(City => City.CityId == hotel.CityId).First();
                var country = _context.Country.Where(c => c.CountryId == city.CountryId).First();

                if (!string.IsNullOrEmpty(countrySearch))
                {
                    if (country.Name == countrySearch)
                    {
                        offersViewModels.Add(new OfferViewModel()
                        {
                            Country = country,
                            Offer = offer,
                            Hotel = hotel,
                            City = city
                        });

                    }
                }
                else
                {
                    offersViewModels.Add(new OfferViewModel()
                    {
                        Country = country,
                        Offer = offer,
                        Hotel = hotel,
                        City = city
                    });
                }

            }
            ViewBag.Countries = new SelectList(countryQuery.Distinct().ToList());
            ViewBag.Option = (string.IsNullOrEmpty(countrySearch) && countryQuery.Distinct().ToList().Contains(countrySearch)) ? "All" : countrySearch;

            return View(offersViewModels);

        }


        // GET
        public ActionResult Create()
        {
            var offer = new Offer();
            return View(offer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(offer);
        }



        // GET: OfferController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OfferController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int id = 1)
        {
            Offer? offer = _context.Offer.First(of => of.OfferId == id);

            if (offer == null)
                return View("Index");
            else
            {
                var hotel = _context.Hotel.Where(Hotel => Hotel.HotelId == offer.HotelId).First();
                var city = _context.City.Where(City => City.CityId == hotel.CityId).First();
                var country = _context.Country.Where(c => c.CountryId == city.CountryId).First();

                var viewModel = new OfferViewModel()
                {
                    Country = country,
                    Offer = offer,
                    Hotel = hotel,
                    City = city
                };
                return View(viewModel);
            }
        }

        public ActionResult Filter(string country)
        {
            var offers = _context.Offer.Where(o => o.Hotel.City.Country.Name == country);


            if (offers == null)
                return View("Index");
            else
                return View("Index", offers);
        }


        // GET: OfferController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OfferController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
