import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { Order } from 'src/app/modals/order.modal';
import { Restaurant } from 'src/app/modals/restaurant.modal';
import { CustService } from '../cust.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  restaurant: Restaurant = null;
  totalCost = 0;
  viewHistory = false;
  myOrdersHistory: Order[] = [];

  constructor(private custService: CustService,
    private authService: AuthService,
    private router: Router) { }

  calcTotalCost() {
    let cost = 0;
    this.restaurant.dishes.forEach((dish, i) => {
      cost += (+this.restaurant.dishes[i].cost * this.restaurant.dishes[i].quantity);
    });

    return cost;
  }

  calcTotalCostForHistoryDish(dishes: { dishName: string, dishCost: string, dishQuantity: number }[]) {
    let cost = 0;
    dishes.forEach((dish, i) => {
      cost += (+dishes[i].dishCost * dishes[i].dishQuantity);
    });

    return cost;
  }

  totalItems() {
    if (this.restaurant !== null) {
      let items = 0;
      this.restaurant.dishes.forEach((dish, i) => {
        items += (+this.restaurant.dishes[i].quantity);
      });

      return items;
    }
    return 0;
  }

  placeOrder() {
    let user = this.authService.getUser();
    let myOrder: Order = {
      customerId: user.id,
      restaurantId: this.restaurant.id,
      restaurantName: this.restaurant.name,
      dishes: [
        ...this.restaurant.dishes.filter((dish) => {
          return dish.quantity > 0;
        }).map(dish => {
          return {
            dishId: dish.id,
            dishName: dish.name,
            dishCost: dish.cost,
            dishQuantity: dish.quantity
          }
        })
      ],
      orderedDate: new Date().toLocaleString(undefined,{timeZone: "Asia/Kolkata"})
    }

    this.custService.saveMyOrder(myOrder).subscribe(_ => {
      alert("Order is placed and is on the way... Thank you...")
      this.resetRestaurant();
      this.router.navigate(['/cust']);
    });
  }

  getMyOrders() {
    if (!this.viewHistory) {
      this.custService.getOrders().subscribe(orderHistory => {
        this.myOrdersHistory = orderHistory;
      });
    }
    this.viewHistory = !this.viewHistory;
  }

  orderAgain(id: number){
    let order = this.myOrdersHistory[id];
    order.orderedDate = new Date().toLocaleString(undefined,{timeZone: "Asia/Kolkata"});
    this.custService.saveMyOrder(order).subscribe(_ => {
      alert("Order is placed and is on the way... Thank you...");
      this.custService.getOrders().subscribe(orderHistory => {
        this.myOrdersHistory = orderHistory;
        this.router.navigate(['/cust']);
      });
    });
  }

  resetRestaurant(){
    this.custService.resetRestaurant();
  }

  ngOnInit(): void {
    let index = 0;
    this.custService.myRestaurantDataEmmitter.subscribe(restaurant => {
      this.restaurant = restaurant;
      if (restaurant !== null) {
        this.totalCost = this.calcTotalCost();
      } else {
        index === 0 ? this.getMyOrders() : "";
      }
      index += 1;
    });
  }

}
