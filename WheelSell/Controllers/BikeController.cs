using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WheelSell.Models;
using WheelSell.Models.AppDbContext;
using WheelSell.Models.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using ReflectionIT.Mvc.Paging;

namespace WheelSell.Controllers
{
    [Authorize(Roles = "Admin,Executive")]

    public class BikeController : Controller
    {
        private readonly WheelSellDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public BikeViewModel BikeVm { get; set; }
        public BikeController(WheelSellDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
            BikeVm = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike = new Models.Bike()
            };

        }


        [Route("/Bike/Index2")]
        public IActionResult Index2()
        {
            var bikes = _db.Bikes.Include(m => m.Make).Include(m => m.Model);
            return View(bikes.ToList());
        }

        [Route("/Bike/Index/{pageNumber?}")]
        //pageSize is number of records displayed on a page
        public IActionResult Index(int pageNumber = 1, int pageSize = 1)
        {
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var bikes = _db.Bikes.Include(m => m.Make).Include(m => m.Model)
                .Skip(ExcludeRecords)
                .Take(pageSize);

            return View(bikes.ToList());
        }

        [HttpGet]
        [Route("/Bike/Create")]
        public IActionResult Create()
        {
            return View(BikeVm);
        }

        [HttpPost, ActionName("Create")]
        [Route("/Bike/Create/{id?}")]
        //[ValidateAntiForgeryToken]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                BikeVm.Makes = _db.Makes.ToList();
                BikeVm.Models = _db.Models.ToList();
                return View(BikeVm);
            }
            _db.Bikes.Add(BikeVm.Bike);
            UploadImageAvailable();

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("Edit")]
        [Route("/Bike/Edits/{id?}")]
        //[ValidateAntiForgeryToken]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                BikeVm.Makes = _db.Makes.ToList();
                BikeVm.Models = _db.Models.ToList();
                return View(BikeVm);
            }
            _db.Bikes.Update(BikeVm.Bike);
            UploadImageAvailable();

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void UploadImageAvailable()
        {
            var BikeId = BikeVm.Bike.Id;

            string wwwrootPath = _hostingEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            var savedBike = _db.Bikes.Find(BikeId);

            if (files.Count != 0)
            {
                var ImagePath = @"images\bike\";
                var Extension = Path.GetExtension(files[0].FileName);
                var RelativeImagePath = ImagePath + BikeId + Extension;
                var AbsImagePath = Path.Combine(wwwrootPath, RelativeImagePath);

                using (var fileStream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                savedBike.ImagePath = RelativeImagePath;
            }
        }

        [HttpGet]
        [Route("/Bike/Edits/{id?}")]
        public IActionResult Edit(int id)
        {

            BikeVm.Bike = _db.Bikes.SingleOrDefault(b => b.Id == id);

            BikeVm.Models = _db.Models.Where(m => m.MakeID == BikeVm.Bike.MakeId);

            if (BikeVm.Bike == null)
            {
                return NotFound();
            }
            return View(BikeVm);
        }

        //[Route("/Model/Edits/{id?}")]
        //public IActionResult Edit(int id)
        //{
        //    ModelVm.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
        //    if (ModelVm.Model == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(ModelVm);
        //}

        //[HttpPost, ActionName("Edit")]
        //[Route("/Model/Edits/{id?}")]
        //public IActionResult EditPost()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(ModelVm);
        //    }
        //    _db.Update(ModelVm.Model);
        //    _db.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [Route("/Bike/Delete/{id?}")]
        public IActionResult Delete(int id)
        {
            Bike bike = _db.Bikes.Find(id);
            if (bike == null)
            {
                return NotFound();
            }
            _db.Bikes.Remove(bike);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }


    }
}
