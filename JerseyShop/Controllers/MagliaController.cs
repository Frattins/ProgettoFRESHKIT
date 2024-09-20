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
using Microsoft.AspNetCore.Identity;

namespace JerseyShop.Controllers
{
    public class MagliaController : Controller
    {
        private readonly FootballShopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public MagliaController(FootballShopContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }

        // Admin Index - Display all maglie (admin only)
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var maglie = _context.Maglie.ToList();  
            return View(maglie);
        }

        // GET: Maglia/SerieA (visible to all users)
        public IActionResult SerieA()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Serie A" && !m.IsSpeciale)  
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/PremierLeague (visible to all users)
        public IActionResult PremierLeague()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Premier League" && !m.IsSpeciale)  
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/Bundesliga (visible to all users)
        public IActionResult Bundesliga()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Bundesliga" && !m.IsSpeciale)  
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/LaLiga (visible to all users)
        public IActionResult LaLiga()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "La Liga" && !m.IsSpeciale)  
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/Ligue1 (visible to all users)
        public IActionResult Ligue1()
        {
            var maglie = _context.Maglie
                .Where(m => m.Campionato == "Ligue 1" && !m.IsSpeciale)  
                .ToList();
            return View(maglie);
        }

        // GET: Maglia/Speciali (visible to all users)
        public IActionResult Speciali()
        {
            var maglieSpeciali = _context.Maglie
                .Where(m => m.IsSpeciale)  
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
                return RedirectToAction("Index");  
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
            return RedirectToAction("Index");  
        }
        public async Task<IActionResult> Details(int id)
        {
            var maglia = await _context.Maglie
                .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maglia == null)
            {
                return NotFound();
            }

            return View(maglia);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview(int magliaId, int rating, string comment)
        {
            var user = await _userManager.GetUserAsync(User);
            var review = new Review
            {
                MagliaId = magliaId,
                UserId = user.Id,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = magliaId });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var maglia = await _context.Maglie
                .Include(m => m.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (maglia == null)
            {
                return NotFound();
            }

            return View(maglia);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Prezzo,Descrizione,ImmagineUrl,Campionato,Squadra")] Maglia maglia)
        {
            if (id != maglia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maglia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MagliaExists(maglia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(maglia);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReview(int id, int magliaId)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Edit), new { id = magliaId });
        }

        private bool MagliaExists(int id)
        {
            return _context.Maglie.Any(e => e.Id == id);
        }
    }

}


