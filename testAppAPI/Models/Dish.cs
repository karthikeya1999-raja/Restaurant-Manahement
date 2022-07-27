using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testAppAPI.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<DishCost> DishCost { get; set; }
    }
}
