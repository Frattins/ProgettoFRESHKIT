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
        public string UserId { get; set; } 

        public DateTime DataOrdine { get; set; }

        public decimal Totale { get; set; }

        
        public List<DettaglioOrdine> DettagliOrdine { get; set; }
    }
}
