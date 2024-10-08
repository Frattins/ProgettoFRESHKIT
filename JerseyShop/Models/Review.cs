﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JerseyShop.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int MagliaId { get; set; }
        public string UserId { get; set; }
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        [StringLength(1000)]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        
        public Maglia Maglia { get; set; }
        public IdentityUser User { get; set; }
    }
}