import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { RestaurantInfoComponent } from './restaurant-info/restaurant-info.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustRoutingModule } from './cust-routing.module';
import { CartComponent } from './cart/cart.component';

@NgModule({
  declarations: [
    HomeComponent,
    RestaurantInfoComponent,
    CartComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CustRoutingModule
  ]
})
export class CustomerModule { }
