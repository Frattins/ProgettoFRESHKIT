using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JerseyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using JerseyShop.Data;

namespace JerseyShop.Controllers
{
    public class CarrelloController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly FootballShopContext _context;

        public CarrelloController(UserManager<IdentityUser> userManager, FootballShopContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        private string GetCarrelloSessionKey()
        {
            var userId = _userManager.GetUserId(User); // Ottiene l'ID dell'utente attuale
            return $"Carrello_{userId}"; // Carrello specifico per ogni utente
        }

        public IActionResult Index()
        {
            var carrello = HttpContext.Session.GetObject<Carrello>(GetCarrelloSessionKey()) ?? new Carrello();
            return View(carrello);
        }

        // Modifica: Aggiungi accetta POST con supporto per la personalizzazione
        [HttpPost]
        public IActionResult Aggiungi(int magliaId, string customName, int? customNumber)
        {
            var carrello = HttpContext.Session.GetObject<Carrello>(GetCarrelloSessionKey()) ?? new Carrello();
            var maglia = GetMagliaById(magliaId);

            if (maglia == null)
            {
                return NotFound();
            }

            // Calcola il prezzo, aggiungendo 20 euro solo se ci sono personalizzazioni
            decimal prezzoUnitario = maglia.Prezzo;
            if (!string.IsNullOrEmpty(customName) || customNumber.HasValue)
            {
                prezzoUnitario += 20;
            }

            var item = carrello.Items.FirstOrDefault(i => i.MagliaId == magliaId &&
                                                          i.CustomName == customName &&
                                                          i.CustomNumber == customNumber);

            if (item == null)
            {
                carrello.Items.Add(new CarrelloItem
                {
                    MagliaId = magliaId,
                    Nome = maglia.Nome,
                    PrezzoUnitario = prezzoUnitario,
                    Quantità = 1,
                    ImmagineUrl = maglia.ImmagineUrl,
                    CustomName = customName,
                    CustomNumber = customNumber
                });
            }
            else
            {
                item.Quantità++;
            }

            HttpContext.Session.SetObject(GetCarrelloSessionKey(), carrello);

            return RedirectToAction("Index");
        }



        // Rimozione di una maglia dal carrello
        [HttpPost]
        public IActionResult Rimuovi(int magliaId)
        {
            var carrello = HttpContext.Session.GetObject<Carrello>(GetCarrelloSessionKey()) ?? new Carrello();
            var item = carrello.Items.FirstOrDefault(i => i.MagliaId == magliaId);

            if (item != null)
            {
                carrello.Items.Remove(item);
                HttpContext.Session.SetObject(GetCarrelloSessionKey(), carrello);
            }

            return RedirectToAction("Index");
        }

        // Incrementa la quantità
        [HttpPost]
        public IActionResult IncrementaQuantita(int magliaId)
        {
            var carrello = HttpContext.Session.GetObject<Carrello>(GetCarrelloSessionKey()) ?? new Carrello();
            var item = carrello.Items.FirstOrDefault(i => i.MagliaId == magliaId);

            if (item != null)
            {
                item.Quantità++;
                HttpContext.Session.SetObject(GetCarrelloSessionKey(), carrello);
            }

            return RedirectToAction("Index");
        }

        // Decrementa la quantità
        [HttpPost]
        public IActionResult DecrementaQuantita(int magliaId)
        {
            var carrello = HttpContext.Session.GetObject<Carrello>(GetCarrelloSessionKey()) ?? new Carrello();
            var item = carrello.Items.FirstOrDefault(i => i.MagliaId == magliaId);

            if (item != null && item.Quantità > 1)
            {
                item.Quantità--;
                HttpContext.Session.SetObject(GetCarrelloSessionKey(), carrello);
            }

            return RedirectToAction("Index");
        }

        private Maglia GetMagliaById(int magliaId)
        {
            var maglia = _context.Maglie.FirstOrDefault(m => m.Id == magliaId);

            if (maglia == null)
            {
                throw new Exception("Maglia non trovata"); // Puoi gestire l'eccezione in modo più appropriato
            }

            return maglia;
        }
    }
}
