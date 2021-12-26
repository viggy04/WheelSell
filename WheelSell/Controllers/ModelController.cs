using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WheelSell.Controllers.Resources;
using WheelSell.Models;
using WheelSell.Models.AppDbContext;
using WheelSell.Models.ViewModels;

namespace WheelSell.Controllers
{
    [Authorize(Roles = "Admin,Executive")]

    public class ModelController : Controller
    {
        private readonly WheelSellDbContext _db;
        private readonly IMapper _mapper;

        [BindProperty]
        public ModelViewModel ModelVm { get; set; } 
        public ModelController(WheelSellDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;

            ModelVm = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Model = new Models.Model()
            };
        }


        [Route("/Model/Index")]
        public IActionResult Index()
        {
            var model = _db.Models.Include(m => m.Make);
            return View(model);
        }

        [HttpGet]
        [Route("/Model/Create")]
        public IActionResult Create()
        {
            return View(ModelVm);
        }

        [HttpPost,ActionName("Create")]
        [Route("/Model/Create/{id?}")]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVm);
            }
            _db.Models.Add(ModelVm.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Route("/Model/Edits/{id?}")]
        public IActionResult Edit(int id)
        {
            ModelVm.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if(ModelVm.Model == null)
            {
                return NotFound();
            }
            return View(ModelVm);
        }

        [HttpPost,ActionName("Edit")]
        [Route("/Model/Edits/{id?}")]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                return View(ModelVm);
            }
            _db.Update(ModelVm.Model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [Route("/Model/Delete/{id?}")]
        public IActionResult Delete(int id)
        {
            Model model = _db.Models.Find(id);
            if (model == null)
            {
                return NotFound();
            }
            _db.Models.Remove(model);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        [AllowAnonymous]
        //[HttpGet("api/models/{MakeID}")]
        [Route("/api/models/{MakeID}")]
        public IEnumerable<Model> Models(int MakeID)
        {
            return _db.Models.ToList()
                .Where(m=>m.MakeID==MakeID);
        }

        //[HttpGet("api/models")]        
        [Route("/api/models")]
        [AllowAnonymous]
        public IEnumerable<ModelResources> Models()
        {
            var models = _db.Models.ToList();

            return _mapper.Map<List<Model>, List<ModelResources>>(models); 
            //var modelResource = models.Select(m => new ModelResources
            //{
            //    Id = m.Id,
            //    Name = m.Name
            //}).ToList();
            //return _db.Models.ToList();
            //return modelResource;
        } 
    }
}
