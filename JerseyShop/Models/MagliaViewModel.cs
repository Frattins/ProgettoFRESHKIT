namespace JerseyShop.Models
{

    public class MagliaViewModel
    {
        public string Nome { get; set; }
        public decimal Prezzo { get; set; }
        public string Descrizione { get; set; }
        public string Campionato { get; set; }
        public string Squadra { get; set; }
        public IFormFile ImmagineUrl { get; set; } 
        public bool IsSpeciale { get; set; }
    }
}