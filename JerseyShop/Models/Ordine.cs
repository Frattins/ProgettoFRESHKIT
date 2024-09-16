using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JerseyShop.Models
{
    public class Ordine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Id dell'utente che ha fatto l'ordine

        public DateTime DataOrdine { get; set; }

        public decimal Totale { get; set; }

        // Relazione con DettagliOrdine
        public List<DettaglioOrdine> DettagliOrdine { get; set; }
    }
}
