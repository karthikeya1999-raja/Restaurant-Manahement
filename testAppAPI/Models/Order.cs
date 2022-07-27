using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testAppAPI.Models
{
    public class Order
    {
        [Key]
        [Column(TypeName = "int")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int CustomerId { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int RestaurantId { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int DishId { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int OrderQuantity { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string OrderDate { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        [ForeignKey("DishId")]
        public Dish Dish { get; set; }
        [ForeignKey("RestaurantId")]
        public Restaurant Restaurant { get; set; }
    }
}
