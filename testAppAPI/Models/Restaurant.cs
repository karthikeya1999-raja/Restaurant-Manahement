using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace testAppAPI.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(7)")]
        public string Type { get; set; }
        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string ImgUrl { get; set; }

        public virtual ICollection<Order> Order { get; set; }
        public virtual ICollection<DishCost> DishCost { get; set; }
    }
}
