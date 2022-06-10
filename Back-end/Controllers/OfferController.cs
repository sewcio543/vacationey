using Backend.Models;
using Backend.Models.DbModels;
using Backend.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult Index(string countrySearch, string sortOrder, string cityFrom, string cityTo)
        {

            IQueryable<string> countryQuery = from c in _context.Country
                                              where c.Name != countrySearch
                                              orderby c.Name
                                              select c.Name;

            IQueryable<string> cityQuery = from c in _context.City
                                           where c.Name != cityFrom && c.Name != cityTo
                                           orderby c.Name
                                           select c.Name;



            var offers = from o in _context.Offer
                                        select o;

            switch (sortOrder)
            {
                case "Descending":
                    offers = offers.OrderByDescending(s => s.Price);
                    break;
                case "Ascending":
                    offers = offers.OrderBy(s => s.Price);
                    break;
            }

            List<OfferViewModel> offersViewModels = new List<OfferViewModel>();

            foreach (var offer in offers)
            {
                var hotel = _context.Hotel.Where(Hotel => Hotel.HotelId == offer.HotelId).First();
                var city = _context.City.Where(City => City.CityId == hotel.CityId).First();
                var cityDep = _context.City.Where(City => City.CityId == offer.DepartureCityId).First();


                var country = _context.Country.Where(c => c.CountryId == city.CountryId).First();


                if (country.Name == countrySearch || string.IsNullOrEmpty(countrySearch))
                {
                    if (cityDep.Name == cityFrom || string.IsNullOrEmpty(cityFrom))
                    {
                        if (city.Name == cityTo || string.IsNullOrEmpty(cityTo))
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
                }
            }

            ViewBag.Country = string.IsNullOrEmpty(countrySearch) ? "All" : countrySearch;
            ViewBag.Order = string.IsNullOrEmpty(sortOrder) ? "" : sortOrder;
            ViewBag.CityFrom = string.IsNullOrEmpty(cityFrom) ? "All" : cityFrom;
            ViewBag.CityTo = string.IsNullOrEmpty(cityTo) ? "All" : cityTo;

            ViewBag.Countries = new SelectList(countryQuery.Distinct().ToList());
            ViewBag.CitiesFrom = new SelectList(cityQuery.Distinct().ToList());



            if (!string.IsNullOrEmpty(countrySearch))
            {
                var countryId = _context.Country.Where(c => c.Name == countrySearch).First().CountryId;
                cityQuery = from c in _context.City
                            join cn in _context.Country on c.CountryId equals cn.CountryId
                            where c.Name != cityFrom && c.Name != cityTo && c.CountryId == countryId
                            orderby c.Name
                            select c.Name;
            }

            ViewBag.CitiesTo = new SelectList(cityQuery.Distinct().ToList());

            return View(offersViewModels);
        }




        // GET
        [Authorize]
        public ActionResult Create()
        {
            IQueryable<string> cityQuery = from c in _context.City
                                           orderby c.Name
                                           select c.Name;

            IQueryable<string> hotelQuery = from h in _context.Hotel
                                            orderby h.Name
                                            select h.Name;

            ViewBag.Cities = new SelectList(cityQuery.Distinct().ToList());
            ViewBag.Hotels = new SelectList(hotelQuery.Distinct().ToList());

            var model = new CreateOfferViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateOfferViewModel offerModel)
        {
            IQueryable<string> cityQuery = from c in _context.City
                                           orderby c.Name
                                           select c.Name;

            IQueryable<string> hotelQuery = from h in _context.Hotel
                                            orderby h.Name
                                            select h.Name;

            var hotelId = _context.Hotel.Where(Hotel => Hotel.Name == offerModel.Hotel).First().HotelId;
            var cityId = _context.City.Where(City => City.Name == offerModel.DepartureCity).First().CityId;

            var offer = new Offer
            {
                HotelId = hotelId,
                DepartureCityId = cityId,
                Price = (decimal)offerModel.Price,
                DateTo = offerModel.DateTo,
                DateFrom = offerModel.DateFrom,
                FullBoard = offerModel.FullBoard
            };

            if (ModelState.IsValid)
            {
                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Cities = new SelectList(cityQuery.Distinct().ToList());
            ViewBag.Hotels = new SelectList(hotelQuery.Distinct().ToList());
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
