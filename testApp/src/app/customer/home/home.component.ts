import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Restaurant } from 'src/app/modals/restaurant.modal';
import { AuthService } from 'src/app/auth/auth.service';
import { CustService } from '../cust.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  searchForm = new FormGroup({
    filter: new FormControl('')
  })

  restaurants: Restaurant[] = [];
  filteredRestaurants: Restaurant[] = [];
  constructor(private custService: CustService,
    private router: Router,
    private authService: AuthService) { }

  getRestaurants(){
    this.custService.getRestaurants().subscribe(restaurants => {
      this.restaurants = this.filteredRestaurants = restaurants;
    });
  }

  filter(){
    this.filteredRestaurants = this.restaurants.filter(restaurant => {
      return restaurant.type === this.searchForm.value.filter;
    });
  }

  orderHere(id: number){
    this.router.navigate(['/cust/restaurant-info',id]);
  }

  ngOnInit(): void {
    if(this.authService.isLOggedIn()){
      this.getRestaurants();
    }else{
      alert("Not Authorized...");
      this.router.navigate(['/login']);
    }
  }

}
