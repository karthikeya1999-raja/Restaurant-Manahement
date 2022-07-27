import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { AdminService } from '../admin.service';
import { Restaurant } from '../../modals/restaurant.modal';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-add-restaurant',
  templateUrl: './add-restaurant.component.html',
  styleUrls: ['./add-restaurant.component.css']
})
export class AddRestaurantComponent implements OnInit {

  restaurantForm = new FormGroup({
    name: new FormControl('',[Validators.required]),
    type: new FormControl('',[Validators.required]),
    imgUrl: new FormControl('',[Validators.required]),
    dishes: new FormArray([
      this.createDishFormGroup()
    ])
  });

  inEditMode = false;
  id = -1;

  constructor(private adminService: AdminService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router) { }

  get dishes(){
    return this.restaurantForm.get('dishes') as FormArray;
  }

  createDishFormGroup(){
    return new FormGroup({
      name: new FormControl('',[Validators.required]),
      imgUrl: new FormControl('',[Validators.required]),
      cost: new FormControl('',[Validators.required])
    });
  }

  addDish(){
    this.dishes.push(this.createDishFormGroup());
  }

  deleteDish(i: number){
    this.dishes.removeAt(i);
  }

  clear(){
    this.restaurantForm.reset();
  }

  submit(){
    let myRestaurant: Restaurant = {
      ...this.restaurantForm.value,
      id: this.id
    };
    let confirmObservable: Observable<Restaurant>;
    if(this.inEditMode){
      confirmObservable =  this.adminService.editRestaurantInfo(this.id,myRestaurant);
    }else{
      confirmObservable = this.adminService.addRestaurantInfo(myRestaurant);
    }
    confirmObservable.subscribe(_ => {
      this.router.navigate(['/admin']);
    });
  }

  initForm(restaurant: Restaurant){
    while(restaurant.dishes.length !== this.restaurantForm.value.dishes.length){
      this.addDish();
    }
    this.restaurantForm.patchValue(restaurant);
  }

  ngOnInit(): void {
    if(this.authService.isAdmin()){
      this.route.params.subscribe(parms => {
        this.id = parms['id'];
        this.inEditMode = this.id === undefined? false : true;
        if(this.inEditMode){
          this.adminService.getRestaurant(this.id).subscribe(restaurant => {
            this.initForm(restaurant); 
          });
        }
      });
    }else{
      alert("Not Authorized...");
      this.router.navigate(['/login']);
    }
  }
}
