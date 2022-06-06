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

        public async Task<IActionResult> Index(string country, string searchString)
        {
            IQueryable<string> countryQuery = from c in _context.Country
                                              select c.Name;

            var offers = _context.Offer;

            //if (!string.IsNullOrEmpty(searchString))
            //{
            //    offers = offers.Where(s => s.Hotel.City.Country.Name!.Contains(searchString));
            //}

            //if (!string.IsNullOrEmpty(country))
            //{
            //    offers = offers.Where(x => x.Hotel.City.Country.Name == country);
            //}

            List<OfferViewModel> offersViewModels = new List<OfferViewModel>();

            foreach(var offer in offers)
            {
                var hotel = _context.Hotel.Where(Hotel => Hotel.HotelId == offer.HotelId).First();
                var city = _context.City.Where(City => City.CityId == hotel.CityId).First();


                offersViewModels.Add(new OfferViewModel()
                {
                    Country = new Country("Spain"),
                    Offer = offer,
                    Hotel = hotel,
                    City = city
                }); 
            }
            

            var countryOfferVM = new CountryOfferViewModel()
            {
                Countries = new SelectList(await countryQuery.Distinct().ToListAsync()),
                Offers = await offers.ToListAsync()
            };

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

        public ActionResult Details(int id)
        {
            Offer? offer = _context.Offer.First(of => of.OfferId == id);

            if (offer == null)
                return View("Index");
            else
                return View(offer);
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
