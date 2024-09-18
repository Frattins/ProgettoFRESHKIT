using Microsoft.AspNetCore.Mvc;
using JerseyShop.Models;
using JerseyShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JerseyShop.Controllers
{
    public class MagliaController : Controller
    {
        private readonly FootballShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MagliaController(FootballShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Admin Index - Display all maglie (admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var maglie = _context.Maglie.ToList();  // Show all jerseys, including special and from all leagues
            return View(maglie);
        }

        // GET: Maglia/SerieA (visible to all users)
        public IActionResult SerieA()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Serie A" && !m.IsSpeciale)  // Exclude special jerseys
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/PremierLeague (visible to all users)
        public IActionResult PremierLeague()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Premier League" && !m.IsSpeciale)  // Exclude special jerseys
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/Bundesliga (visible to all users)
        public IActionResult Bundesliga()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Bundesliga" && !m.IsSpeciale)  // Exclude special jerseys
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/LaLiga (visible to all users)
        public IActionResult LaLiga()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "La Liga" && !m.IsSpeciale)  // Exclude special jerseys
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/Ligue1 (visible to all users)
        public IActionResult Ligue1()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Ligue 1" && !m.IsSpeciale)  // Exclude special jerseys
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/Speciali (visible to all users)
        public IActionResult Speciali()
        {
            var maglieSpeciali = _context.Maglie
                .Where(m => m.IsSpeciale)  // Show only special jerseys
                .ToList();
            return View(maglieSpeciali);
        }

        // GET: Maglia/Create (admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            // Pass available leagues to the view
            ViewBag.Campionati = new SelectList(new[] { "Premier League", "Serie A", "La Liga", "Bundesliga", "Ligue 1" });
            return View();
        }

        // POST: Maglia/Create (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MagliaViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // Check if an image has been uploaded
                if (model.ImmagineUrl != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Maglie");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImmagineUrl.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImmagineUrl.CopyToAsync(fileStream);
                    }
                }

                var newMaglia = new Maglia
                {
                    Nome = model.Nome,
                    Prezzo = model.Prezzo,
                    Descrizione = model.Descrizione,
                    ImmagineUrl = uniqueFileName,
                    IsSpeciale = model.IsSpeciale,
                    Campionato = model.Campionato,
                    Squadra = model.Squadra
                };

                _context.Add(newMaglia);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");  // Redirect to the admin index after creation
            }

            return View(model);
        }

        // DELETE: Maglia/Delete (admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var maglia = await _context.Maglie.FindAsync(id);
            if (maglia != null)
            {
                _context.Maglie.Remove(maglia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");  // Redirect to the admin index after deletion
        }
        public async Task<IActionResult> Details(int id)
        {
            var jersey = await _context.Maglie.FindAsync(id);

            if (jersey == null)
            {
                return NotFound();
            }

            return View(jersey);
        }

    }

}
