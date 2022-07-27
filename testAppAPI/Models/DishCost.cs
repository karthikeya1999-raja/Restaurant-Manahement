using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testAppAPI.Models
{
    public class DishCost
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int DishId { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int RestaurantId { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int Cost { get; set; }
        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string ImgUrl { get; set; }

        [ForeignKey("DishId")]
        public Dish Dish { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }
    }
}
