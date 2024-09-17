using Microsoft.AspNetCore.Mvc;
using JerseyShop.Models;
using Microsoft.AspNetCore.Identity;
using JerseyShop.Data;

namespace JerseyShop.Controllers
{
    public class OrdineController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly FootballShopContext _context;

        public OrdineController(UserManager<IdentityUser> userManager, FootballShopContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Checkout()
        {
            var carrelloSessionKey = GetCarrelloSessionKey();
            var carrello = HttpContext.Session.GetObject<Carrello>(carrelloSessionKey);

            if (carrello == null || carrello.Items.Count == 0)
            {
                return RedirectToAction("Index", "Carrello");
            }

            return View(carrello);
        }

        [HttpPost]
        public async Task<IActionResult> ConfermaOrdine()
        {
            var carrelloSessionKey = GetCarrelloSessionKey();
            var carrello = HttpContext.Session.GetObject<Carrello>(carrelloSessionKey);

            if (carrello == null || carrello.Items.Count == 0)
            {
                return RedirectToAction("Index", "Carrello");
            }

            var userId = _userManager.GetUserId(User);
            var ordine = new Ordine
            {
                UserId = userId,
                DataOrdine = DateTime.Now,
                Totale = carrello.Totale,
                DettagliOrdine = carrello.Items.Select(item => new DettaglioOrdine
                {
                    MagliaId = item.MagliaId,
                    Quantità = item.Quantità,
                    PrezzoUnitario = item.PrezzoUnitario
                }).ToList()
            };

            _context.Ordini.Add(ordine);
            await _context.SaveChangesAsync();

            // Svuota il carrello
            HttpContext.Session.Remove(carrelloSessionKey);

            return RedirectToAction("PagamentoCompletato");
        }

        public IActionResult PagamentoCompletato()
        {
            return View();
        }

        // Aggiungi il metodo per un pagamento fittizio
        [HttpPost]
        public IActionResult FakePayment(decimal amount)
        {
            // Simula un ritardo nel pagamento fittizio
            System.Threading.Thread.Sleep(2000);  // Ritardo di 2 secondi
            return RedirectToAction("PagamentoCompletato");
        }

        private string GetCarrelloSessionKey()
        {
            var userId = _userManager.GetUserId(User);
            return $"Carrello_{userId}";
        }
    }
}
