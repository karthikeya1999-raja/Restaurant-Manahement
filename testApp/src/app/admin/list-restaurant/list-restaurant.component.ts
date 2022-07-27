import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { AdminService } from '../admin.service';
import { Restaurant } from '../../modals/restaurant.modal';

@Component({
  selector: 'app-list-restaurant',
  templateUrl: './list-restaurant.component.html',
  styleUrls: ['./list-restaurant.component.css']
})
export class ListRestaurantComponent implements OnInit {

  constructor(private adminService: AdminService,
    private router: Router,
    private authService: AuthService) { }

  restaurants: Restaurant[] = []

  ngOnInit(): void {
    if(this.authService.isAdmin()){
      this.adminService.getRestaurants().subscribe(restaurants => {
        this.restaurants = restaurants;
      });
    }else{
      alert("Not Authorized...");
      this.router.navigate(['/login'])
    }
  }

  addRestaurant(){
    this.router.navigate(['/admin/add-restaurant']);
  }

  deleteRestaurant(id: number){
    this.adminService.deleteRestaurant(id).subscribe(restaurants => {
      this.restaurants = restaurants;
    });
  }

  editRestaurant(id: number){
    this.router.navigate(['/admin/edit-restaurant/',id]);
  }
}
