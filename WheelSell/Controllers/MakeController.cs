using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WheelSell.Models;
using WheelSell.Models.AppDbContext;

namespace WheelSell.Controllers
{
    [Authorize(Roles ="Admin,Executive")]
    public class MakeController : Controller
    {
        private readonly WheelSellDbContext _db;
        public MakeController(WheelSellDbContext db)
        {
            _db = db;
        }

        [Route("")]
        [Route("/Index")]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View(_db.Makes.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/Make/Create")]
        public IActionResult Create(Make make)
        {
            if (ModelState.IsValid)
            {
                _db.Add(make);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }   
            return View(make);
        }

        [HttpPost]
        [Route("/Make/Delete/")]
        public IActionResult Delete(int id)
        {
            var make = _db.Makes.Find(id);
            if(make == null)
            {
                return NotFound();
            }
            _db.Makes.Remove(make);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Route("/Make/Edits/{id?}")]
        public IActionResult Edits(int id)
        {
            var make = _db.Makes.Find(id);
            if(make == null)
            {
                return NotFound();
            }
            return View(make);
        }


        [HttpPost]
        [Route("/Make/Edits/{id?}")]
        public IActionResult Edits(Make make)
        {
            if (ModelState.IsValid)
            {
                _db.Update(make);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(make);
        }


    }
}
