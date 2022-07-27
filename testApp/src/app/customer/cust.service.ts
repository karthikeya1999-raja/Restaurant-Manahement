import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { Order } from '../modals/order.modal';
import { Restaurant } from '../modals/restaurant.modal';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CustService {

  private restaurants: Restaurant[] = [];
  private myRestaurant: Restaurant;
  myRestaurantDataEmmitter = new BehaviorSubject<Restaurant>(null);
  private allOrders: Order[] = [];
  private filteredRestaurants: Restaurant[] = [];
  private BASE_URL = "http://localhost:41223/api/Customer/";

  constructor(private authService: AuthService,
    private http: HttpClient) { }

  getRestaurants(){
    return this.http.get<Restaurant[]>(this.BASE_URL+"restaurants").pipe(map(restaurants => {
      restaurants.sort((a,b) => {
        if (a.name.toLowerCase() > b.name.toLowerCase()){
          return 1;
        }else if(a.name.toLowerCase() == b.name.toLowerCase()){
          return 0;
        }else{
          return -1;
        }
      });
      this.restaurants = restaurants;
      return this.restaurants;
    }));
  }

  resetRestaurant(){
    this.myRestaurantDataEmmitter.next(null);
  }

  getRestaurant(id: number){
    return this.http.get<Restaurant>(this.BASE_URL+"restaurant-info/"+id);
  }

  saveOrderRestaurantData(restaurant: Restaurant){
    this.myRestaurant = restaurant;
    this.myRestaurantDataEmmitter.next(this.myRestaurant);
  }

  getOrders(){
    return this.http.get<Order[]>(this.BASE_URL+"orders/"+this.authService.getUser().id);
  }

  saveMyOrder(order: Order){
    return this.http.post<Order>(this.BASE_URL+"save-order-info",order);
  }
}
