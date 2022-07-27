using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using testAppAPI.Modals;
using testAppAPI.Models;

namespace testAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly testAppAPIDBContext _context;

        public AdminController(testAppAPIDBContext context)
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
            if(restaurant != null)
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
                foreach(var dishCost in dishCostValues)
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

        [HttpPost("add-restaurant-info")]
        public async Task<ActionResult<Restaurant>> AddRestaurant(RestaurantInfoModal restaurantModal)
        {
            var restaurant = new Restaurant
            {
                Name = restaurantModal.Name,
                Type = restaurantModal.Type,
                ImgUrl = restaurantModal.ImgUrl
            };
            await _context.AddAsync(restaurant);
            await _context.SaveChangesAsync();

            var dishes = restaurantModal.Dishes;
            foreach(var dish in dishes)
            {
                var myDish = new Dish
                {
                    Name = dish.Name
                };
                var dishList= await _context.Dishes.ToListAsync();
                dishList = dishList.FindAll(dishItem => dishItem.Name.Trim().ToLower().Equals(myDish.Name.Trim().ToLower()));
                if (dishList.Count == 0)
                {
                    await _context.Dishes.AddAsync(myDish);
                    await _context.SaveChangesAsync();
                }
            }

            foreach (var dish in dishes)
            {
                var dishList = await _context.Dishes.ToListAsync();
                var myDishFromList = dishList.Find(myDish => dish.Name.Trim().ToLower().Equals(myDish.Name.Trim().ToLower()));
                var dishCostList = await _context.DishCost.ToListAsync();
                var dishCost = new DishCost
                {
                    RestaurantId = restaurant.Id,
                    DishId = myDishFromList.Id,
                    Cost = dish.Cost,
                    ImgUrl = dish.ImgUrl
                };
                dishCostList = dishCostList.FindAll(dishCostItem => dishCostItem.DishId == myDishFromList.Id && dishCostItem.RestaurantId == restaurant.Id);
                if (dishCostList.Count == 0)
                {
                    await _context.DishCost.AddAsync(dishCost);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    dishCost = dishCostList[0];
                    dishCost.Cost = dish.Cost;
                    dishCost.ImgUrl = dish.ImgUrl;
                    _context.Entry(dishCost).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok(true);
        }

        [HttpPut("update-restaurant-info/{id}")]
        public async Task<ActionResult<Restaurant>> UpdateRestaurant(int id, RestaurantInfoModal restaurantModal)
        {
            if (id != restaurantModal.Id)
            {
                return BadRequest();
            }

            var restaurant = new Restaurant
            {
                Id = restaurantModal.Id,
                Name = restaurantModal.Name,
                Type = restaurantModal.Type,
                ImgUrl = restaurantModal.ImgUrl
            };
            _context.Entry(restaurant).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var dishes = restaurantModal.Dishes;
            foreach (var dish in dishes)
            {
                var myDish = new Dish
                {
                    Name = dish.Name
                };
                var dishList = await _context.Dishes.ToListAsync();
                dishList = dishList.FindAll(dishItem => dishItem.Name.Trim().ToLower().Equals(myDish.Name.Trim().ToLower()));
                if (dishList.Count == 0)
                {
                    await _context.Dishes.AddAsync(myDish);
                    await _context.SaveChangesAsync();
                }
            }

            foreach (var dish in dishes)
            {
                var dishList = await _context.Dishes.ToListAsync();
                var myDishFromList = dishList.Find(myDish => dish.Name.Trim().ToLower().Equals(myDish.Name.Trim().ToLower()));
                var dishCostList = await _context.DishCost.ToListAsync();
                var dishCost = new DishCost
                {
                    RestaurantId = restaurant.Id,
                    DishId = myDishFromList.Id,
                    Cost = dish.Cost,
                    ImgUrl = dish.ImgUrl
                };
                dishCostList = dishCostList.FindAll(dishCostItem => dishCostItem.DishId == myDishFromList.Id && dishCostItem.RestaurantId == id);
                if(dishCostList.Count == 0)
                {
                    await _context.DishCost.AddAsync(dishCost);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    dishCost = dishCostList[0];
                    dishCost.Cost = dish.Cost;
                    dishCost.ImgUrl = dish.ImgUrl;
                    _context.Entry(dishCost).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok(true);
        }

        [HttpDelete("delete-restaurant-info/{id}")]
        public async Task<ActionResult<Restaurant>> DeleteRestaurant(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if(restaurant != null)
            {
                var orders = await _context.Orders.AsNoTracking().ToListAsync();
                var ordersOfThisRestaurant = orders.FindAll(order => order.RestaurantId == id).ToArray();
                _context.Orders.RemoveRange(ordersOfThisRestaurant);
                await _context.SaveChangesAsync();
                var dishCostValues = await _context.DishCost.AsNoTracking().ToListAsync();
                var dishesOfThisRestaurant = dishCostValues.FindAll(dishCost => dishCost.RestaurantId == id).ToArray();
                _context.DishCost.RemoveRange(dishesOfThisRestaurant);
                await _context.SaveChangesAsync();
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
                return Ok(await _context.Restaurants.ToListAsync());
            }
            return BadRequest("No Restaurant Founfd with ID: "+id);
        }
    }
}
