namespace JerseyShop.Models
{

    public class MagliaViewModel
    {
        public string Nome { get; set; }
        public decimal Prezzo { get; set; }
        public string Descrizione { get; set; }
        public string Campionato { get; set; }
        public string Squadra { get; set; }
        public IFormFile ImmagineUrl { get; set; } // Per il caricamento dell'immagine
        public bool IsSpeciale { get; set; }
    }
}