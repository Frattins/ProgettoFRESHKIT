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
    [Authorize(Roles = "Admin")]
    public class MagliaController : Controller
    {
        private readonly FootballShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MagliaController(FootballShopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string campionato)
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == campionato && !m.IsSpeciale) // Escludi le maglie speciali
                .ToList();
            ViewBag.Campionato = campionato;  // Assicurati che il campionato sia passato correttamente
            return View(maglie);
        }

        // GET: Maglia/SerieA
        public IActionResult SerieA()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Serie A" && !m.IsSpeciale) // Escludi le maglie speciali
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/PremierLeague
        public IActionResult PremierLeague()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Premier League" && !m.IsSpeciale) // Escludi le maglie speciali
                .ToList();
            ViewBag.Campionato = "Premier League";
            return View(maglie);
        }

        // GET: Maglia/Bundesliga
        public IActionResult Bundesliga()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Bundesliga" && !m.IsSpeciale) // Escludi le maglie speciali
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/LaLiga
        public IActionResult LaLiga()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "La Liga" && !m.IsSpeciale) // Escludi le maglie speciali
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/Ligue1
        public IActionResult Ligue1()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Ligue 1" && !m.IsSpeciale) // Escludi le maglie speciali
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/Speciali
        public IActionResult Speciali()
        {
            var maglieSpeciali = _context.Maglie
                .Where(m => m.IsSpeciale)  // Mostra solo le maglie speciali
                .ToList();
            return View(maglieSpeciali);
        }

        // GET: Maglia/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Potresti voler passare alcune informazioni tramite ViewBag
            ViewBag.Campionati = new SelectList(new[] { "Premier League", "Serie A", "La Liga", "Bundesliga", "Ligue 1" });
            return View();
        }

        // POST: Maglia/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MagliaViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // Check if the image has been uploaded
                if (model.ImmagineUrl != null)
                {
                    // Define the folder where the images will be stored
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Maglie");

                    // Ensure that the directory exists, and create it if necessary
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Create a unique filename for the image
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImmagineUrl.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the image file to the specified path
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImmagineUrl.CopyToAsync(fileStream);
                    }
                }

                // Create a new object Maglia with the specified fields
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

                // Add the new jersey to the database
                _context.Add(newMaglia);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var maglia = await _context.Maglie.FindAsync(id);
            if (maglia != null)
            {
                _context.Maglie.Remove(maglia);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
