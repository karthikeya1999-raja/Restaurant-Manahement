import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { Dish } from 'src/app/modals/dish.modal';
import { Restaurant } from 'src/app/modals/restaurant.modal';
import { CustService } from '../cust.service';

@Component({
  selector: 'app-restaurant-info',
  templateUrl: './restaurant-info.component.html',
  styleUrls: ['./restaurant-info.component.css']
})
export class RestaurantInfoComponent implements OnInit {

  restaurant: Restaurant;
  id: number = -1;
  orderedDishs: Dish[];

  constructor(private custService: CustService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router) { }

  add(id: number){
    this.orderedDishs.find(dish => dish.id === id).quantity += 1;
    this.saveMyOrderingRestaurant();
  }

  remove(id: number){
    this.orderedDishs.find(dish => dish.id === id).quantity -= 1;
    this.saveMyOrderingRestaurant();
  }

  saveMyOrderingRestaurant(){
    this.restaurant.dishes = this.orderedDishs;
    this.custService.saveOrderRestaurantData(this.restaurant);
  }

  ngOnInit(): void {
   if(this.authService.isLOggedIn()){
    this.route.params.subscribe(parms => {
      this.id = parms['id'];
      this.custService.getRestaurant(this.id).subscribe(restaurant => {
        this.restaurant = restaurant;
        this.orderedDishs = [
          ...this.restaurant.dishes.map<Dish>((dish: Dish) => {
            dish.quantity = 0;
            return dish
          })
        ]
        this.saveMyOrderingRestaurant();
      });
    });
   }else{
     alert("Not Autherized...");
     this.router.navigate(['/login']);
   }
  }
}
