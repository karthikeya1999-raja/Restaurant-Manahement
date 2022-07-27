import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Restaurant } from '../modals/restaurant.modal';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  private restaurants: Restaurant[] = [];
  private BASE_URL = "http://localhost:41223/api/Admin/";

  constructor(private http: HttpClient) { }

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

  addRestaurantInfo(restaurantInfo: Restaurant){
    return this.http.post<Restaurant>(this.BASE_URL+"add-restaurant-info",restaurantInfo);
  }

  getRestaurant(id: number){
    return this.http.get<Restaurant>(this.BASE_URL+"restaurant-info/"+id);
  }

  deleteRestaurant(id: number){
    return this.http.delete<Restaurant[]>(this.BASE_URL+"delete-restaurant-info/"+id).pipe(map(restaurants => {
      this.restaurants = restaurants;
      return this.restaurants;
    }))
  }

  editRestaurantInfo(id: number, restaurant: Restaurant){
    return this.http.put<Restaurant>(this.BASE_URL+"update-restaurant-info/"+id,restaurant);
  }
}
