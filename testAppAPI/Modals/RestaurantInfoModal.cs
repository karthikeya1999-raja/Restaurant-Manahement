using System.Collections.Generic;

namespace testAppAPI.Modals
{
    public class RestaurantInfoModal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string Type { get; set; }
        public List<DishModal> Dishes { get; set; }
    }
}
