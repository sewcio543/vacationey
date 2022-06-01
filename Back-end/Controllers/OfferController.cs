using Backend.Models.DbModels;
using BookApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class OfferController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        // GET: OfferController/Details/5
        public ActionResult ViewAll()
        {
            return View();
        }

        // GET
        public ActionResult Create()
        {
            var offer = new Offer();
            return View(offer);
        }

        [HttpPost]
        public ActionResult Create(Offer offer)
        {
            if (ModelState.IsValid)
            {
                using (DatabaseContext db = new DatabaseContext())
                {
                    db.Offer.Add(offer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
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
    }
}
