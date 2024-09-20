using System.ComponentModel.DataAnnotations;

namespace JerseyShop.Models
{
    public class DettaglioOrdine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrdineId { get; set; }

        [Required]
        public int MagliaId { get; set; }

        public int Quantità { get; set; }

        public decimal PrezzoUnitario { get; set; }
        public Ordine Ordine { get; set; }
        public Maglia Maglia { get; set; }
    }
}
