using JerseyShop.Data;
using JerseyShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace JerseyShop.Controllers
{
    public class CarrelloController : Controller
    {
        private readonly FootballShopContext _context;

        public CarrelloController(FootballShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var carrello = GetCarrello();
            return View(carrello);
        }

        public IActionResult Add(int id)
        {
            var maglia = _context.Maglie.Find(id);
            if (maglia != null)
            {
                var carrello = GetCarrello();
                var item = carrello.Items.FirstOrDefault(i => i.MagliaId == id);
                if (item == null)
                {
                    carrello.Items.Add(new CarrelloItem
                    {
                        MagliaId = maglia.Id,
                        Nome = maglia.Nome,
                        PrezzoUnitario = maglia.Prezzo,
                        Quantità = 1
                    });
                }
                else
                {
                    item.Quantità++;
                }
                SaveCarrello(carrello);
            }
            return RedirectToAction("Index");
        }

        private Carrello GetCarrello()
        {
            var carrello = HttpContext.Session.GetObject<Carrello>("Carrello") ?? new Carrello();
            return carrello;
        }

        private void SaveCarrello(Carrello carrello)
        {
            HttpContext.Session.SetObject("Carrello", carrello);
        }
    }
}
