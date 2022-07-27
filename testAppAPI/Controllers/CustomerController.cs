using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testAppAPI.Modals;
using testAppAPI.Models;

namespace testAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly testAppAPIDBContext _context;

        public CustomerController(testAppAPIDBContext context)
        {
            _context = context;
        }

        [HttpGet("restaurants")]
        public async Task<ActionResult<Restaurant>> GetRestaurants()
        {
            var restaurants = await _context.Restaurants.AsNoTracking().ToListAsync();
            return Ok(restaurants);
        }

        [HttpGet("restaurant-info/{id}")]
        public async Task<ActionResult<RestaurantInfoModal>> GetRestaurant(int id)
        {
            List<DishModal> dishModalList = new List<DishModal>();
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                var restaurantModal = new RestaurantInfoModal() 
                {
                    Id = restaurant.Id,
                    Name = restaurant.Name,
                    Type = restaurant.Type,
                    ImgUrl = restaurant.ImgUrl
                };
                var dishCostValues = await _context.DishCost.AsNoTracking().ToListAsync();
                dishCostValues = dishCostValues.FindAll(dishCost => dishCost.RestaurantId == id);
                foreach (var dishCost in dishCostValues)
                {
                    var dishes = await _context.Dishes.AsNoTracking().ToListAsync();
                    var dish = dishes.Find(dish => dish.Id == dishCost.DishId);
                    var dishModal = new DishModal()
                    { 
                        Id = dish.Id,
                        Name = dish.Name,
                        ImgUrl = dishCost.ImgUrl,
                        Cost = dishCost.Cost
                    };
                    dishModalList.Add(dishModal);
                }
                restaurantModal.Dishes = dishModalList;
                return Ok(restaurantModal);
            }
            return BadRequest();
        }

        [HttpGet("orders/{id}")]
        public async Task<ActionResult<IEnumerable<OrderInfoModal>>> GetOrders(int id)
        {
            var map = new Dictionary<int, Dictionary<string,List<OrderDishModal>>>();
            List<OrderInfoModal> orderModalList = new List<OrderInfoModal>();
            var orders = await _context.Orders.AsNoTracking().ToListAsync();
            orders = orders.FindAll(order => order.CustomerId == id);
            foreach (var order in orders)
            {
                var orderDishModal = new OrderDishModal()
                {
                    DishId = order.DishId,
                    DishName = _context.Dishes.Where(dishItem => dishItem.Id == order.DishId).Select(d => d.Name).FirstOrDefault(),
                    DishCost = _context.DishCost.Where(dishCostItem => dishCostItem.DishId == order.DishId && dishCostItem.RestaurantId == order.RestaurantId)
                                .Select(d => d.Cost).FirstOrDefault(),
                    DishQuantity = order.OrderQuantity
                };

                if (map.ContainsKey(order.RestaurantId))
                {
                    var restaurantInfo = map[order.RestaurantId];
                    if (restaurantInfo.ContainsKey(order.OrderDate))
                    {
                        var dateInfo = restaurantInfo[order.OrderDate];
                        dateInfo.Add(orderDishModal);
                    }
                    else
                    {
                        restaurantInfo.Add(order.OrderDate,new List<OrderDishModal> { orderDishModal });
                    }
                }
                else
                {
                    map.Add(order.RestaurantId, new Dictionary<string, List<OrderDishModal>>()
                    {
                        { order.OrderDate, new List<OrderDishModal> { orderDishModal } }
                    });
                }
            }

            foreach(var mapItem in map)
            {
                foreach(var orderItem in mapItem.Value)
                {
                    OrderInfoModal orderModal = new OrderInfoModal() 
                    {
                        CustomerId = id,
                        RestaurantId = mapItem.Key,
                        RestaurantName = _context.Restaurants.Where(r => r.Id == mapItem.Key).Select(s => s.Name).FirstOrDefault(),
                        OrderedDate = orderItem.Key,
                        Dishes = orderItem.Value
                    };
                    orderModalList.Add(orderModal);
                }
            }
            return Ok(orderModalList);
        }

        [HttpPost("save-order-info")]
        public async Task<IActionResult> SaveOrderInfo(OrderInfoModal orderModal)
        {
            foreach(var dish in orderModal.Dishes)
            {
                var order = new Order()
                {
                    CustomerId = orderModal.CustomerId,
                    RestaurantId = orderModal.RestaurantId,
                    DishId = dish.DishId,
                    OrderQuantity = dish.DishQuantity,
                    OrderDate = orderModal.OrderedDate
                };
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            return Ok(true);
        }
    }
}
