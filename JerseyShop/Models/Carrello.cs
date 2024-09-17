using System.Collections.Generic;

namespace JerseyShop.Models
{
    public class Carrello
    {
        public List<CarrelloItem> Items { get; set; } = new List<CarrelloItem>();
        public decimal Totale => Items.Sum(i => i.PrezzoTotale);
    }

    public class CarrelloItem
    {
        public int MagliaId { get; set; }
        public string Nome { get; set; }
        public decimal PrezzoUnitario { get; set; }
        public int Quantità { get; set; }
        public decimal PrezzoTotale => PrezzoUnitario * Quantità;
        public string ImmagineUrl { get; set; } 
    }
}
