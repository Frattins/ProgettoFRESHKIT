﻿using System.ComponentModel.DataAnnotations;

namespace JerseyShop.Models
{

    public class Maglia
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public decimal Prezzo { get; set; }

        public string Descrizione { get; set; }

        public string ImmagineUrl { get; set; }

        public bool IsSpeciale { get; set; }

        
        [Required]
        public string Campionato { get; set; }

        [Required]
        public string Squadra { get; set; }
        public bool IsFavorite { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}