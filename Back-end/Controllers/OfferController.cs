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
        private readonly IQueryable<string> cities;
        private readonly IQueryable<string> hotels;
        private readonly IQueryable<string> countries;
        private readonly string[] orders;


        public OfferController(DatabaseContext context)
        {
            _context = context;
            cities = from c in _context.City
                     orderby c.Name
                     select c.Name;

            hotels = from h in _context.Hotel
                     orderby h.Name
                     select h.Name;

            countries = from c in _context.Country
                        orderby c.Name
                        select c.Name;

            orders = new string[] { "Ascending", "Descending" };
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string countrySearch, string sortOrder, string cityFrom, string cityTo, int hotelId, int page = 1)
        {

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

            List<OfferViewModel> offerViewModels = new List<OfferViewModel>();

            foreach (var offer in offers)
            {

                var hotel = _context.Hotel.Find(offer.HotelId);
                var city = _context.City.Find(hotel.CityId);
                var cityDep = _context.City.Find(offer.DepartureCityId);
                var country = _context.Country.First(c => c.CountryId == city.CountryId);


                if (country.Name == countrySearch || string.IsNullOrEmpty(countrySearch))
                {
                    if (cityDep.Name == cityFrom || string.IsNullOrEmpty(cityFrom))
                    {
                        if (city.Name == cityTo || string.IsNullOrEmpty(cityTo))
                        {
                            if (hotel.HotelId == hotelId || hotelId == 0)
                            {
                                offerViewModels.Add(new OfferViewModel()
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
            }

            ViewBag.Orders = new SelectList(orders, (string.IsNullOrEmpty(sortOrder) || !orders.Contains(sortOrder)) ? "" : sortOrder);
            ViewBag.Countries = new SelectList(countries, (string.IsNullOrEmpty(countrySearch) || !countries.Contains(countrySearch)) ? "All" : countrySearch);
            ViewBag.CitiesFrom = new SelectList(cities, (string.IsNullOrEmpty(cityFrom) || !cities.Contains(cityFrom)) ? "All" : cityFrom);
            ViewBag.CitiesTo = new SelectList(cities, (string.IsNullOrEmpty(cityTo) || !cities.Contains(cityTo)) ? "All" : cityTo);

            // cities only from selected country
            if (!string.IsNullOrEmpty(countrySearch))
            {
                try
                {
                    var countryId = _context.Country.First(c => c.Name == countrySearch).CountryId;

                    var cityQuery = from c in _context.City
                                    join cn in _context.Country on c.CountryId equals cn.CountryId
                                    where c.Name != cityFrom && c.Name != cityTo && c.CountryId == countryId
                                    orderby c.Name
                                    select c.Name;

                    ViewBag.CitiesTo = new SelectList(cityQuery, (string.IsNullOrEmpty(cityTo) || !cities.Contains(cityTo)) ? "All" : cityTo);
                }
                catch (InvalidOperationException) { }
            }

            // pagination
            offerViewModels = offerViewModels.Skip((page - 1) * 10).Take(10).ToList();

            return View(offerViewModels);
        }


        // GET
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Cities = new SelectList(cities.Distinct().ToList());
            ViewBag.Hotels = new SelectList(hotels.Distinct().ToList());

            var model = new CreateOfferViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreateOfferViewModel offerModel)
        {
            var hotelId = _context.Hotel.First(Hotel => Hotel.Name == offerModel.Hotel).HotelId;
            var cityId = _context.City.First(City => City.Name == offerModel.DepartureCity).CityId;

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

            ViewBag.Cities = new SelectList(cities.Distinct().ToList());
            ViewBag.Hotels = new SelectList(hotels.Distinct().ToList());
            return View(offerModel);

        }

        public ActionResult Edit(int id)
        {
            var offer = _context.Offer.Find(id);

            if (offer == null)
                return View("Index", _context.Offer);

            ViewBag.Cities = new SelectList(cities.Distinct().ToList());
            ViewBag.Hotels = new SelectList(hotels.Distinct().ToList());
            ViewBag.ID = id;

            var viewModel = GenerateCreateOfferViewModel(id);

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(CreateOfferViewModel newModel)
        {
            var offer = _context.Offer.Find(newModel.OfferId);
            var hotelId = _context.Hotel.First(Hotel => Hotel.Name == newModel.Hotel).HotelId;
            var cityId = _context.City.First(City => City.Name == newModel.DepartureCity).CityId;

            offer.Price = (decimal)newModel.Price;
            offer.DateTo = newModel.DateTo;
            offer.DateFrom = newModel.DateFrom;
            offer.FullBoard = newModel.FullBoard;
            offer.DepartureCityId = cityId;
            offer.HotelId = hotelId;

            if (ModelState.IsValid)
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ID = newModel.OfferId;
            ViewBag.Cities = new SelectList(cities.Distinct().ToList());
            ViewBag.Hotels = new SelectList(hotels.Distinct().ToList());

            return View(GenerateCreateOfferViewModel(newModel.OfferId));
        }

        public IActionResult Details(int id = 1)
        {
            try
            {
                var offer = _context.Offer.First(of => of.OfferId == id);
            }
            catch
            {
                return View("Index", _context.Offer);
            }

            var viewModel = GenerateOfferViewModel(id);
            return View(viewModel);

        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var offer = _context.Offer.First(of => of.OfferId == id);

            if (offer == null)
                return View("Index");
            else
            {
                var viewModel = GenerateOfferViewModel(id);
                return View(viewModel);
            }
        }

        // POST: OfferController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            var offer = _context.Offer.First(of => of.OfferId == id);

            try
            {
                _context.Offer.Remove(offer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Index", _context.Offer);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //
        public OfferViewModel GenerateOfferViewModel(int offerId)
        {
            var offer = _context.Offer.Find(offerId);
            var hotel = _context.Hotel.Find(offer.HotelId);
            var city = _context.City.Find(hotel.CityId);
            var country = _context.Country.Find(city.CountryId);


            var viewModel = new OfferViewModel()
            {
                Country = country,
                Offer = offer,
                Hotel = hotel,
                City = city
            };
            return viewModel;
        }

        public CreateOfferViewModel GenerateCreateOfferViewModel(int offerId)
        {
            var offer = _context.Offer.Find(offerId);
            var hotel = _context.Hotel.Find(offer.HotelId);
            var city = _context.City.Find(hotel.CityId);
            var country = _context.Country.Find(city.CountryId);
            var cityDep = _context.City.Find(offer.DepartureCityId);


            var viewModel = new CreateOfferViewModel()
            {
                OfferId = offerId,
                Hotel = hotel.Name,
                Price = (double)offer.Price,
                DepartureCity = cityDep.Name,
                DateFrom = offer.DateFrom,
                DateTo = offer.DateTo,
                FullBoard = offer.FullBoard
            };
            return viewModel;
        }
    }
}