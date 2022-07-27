using System.Collections.Generic;

namespace testAppAPI.Modals
{
    public class OrderInfoModal
    {
        public int CustomerId { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public List<OrderDishModal> Dishes { get; set; }
        public string OrderedDate { get; set; }

    }
}
