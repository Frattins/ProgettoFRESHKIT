using System.ComponentModel.DataAnnotations;

namespace JerseyShop.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public int MagliaId { get; set; }
        public Maglia Maglia { get; set; }
    }
}