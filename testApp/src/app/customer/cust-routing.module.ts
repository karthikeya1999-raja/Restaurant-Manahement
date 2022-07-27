import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RestaurantInfoComponent } from './restaurant-info/restaurant-info.component';

const routes: Routes = [
  {path: "cust", children: [
      {path: "", redirectTo: "home", pathMatch: "full"},
      {path: "home", component: HomeComponent},
      {path: "restaurant-info/:id", component: RestaurantInfoComponent}
  ]},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class CustRoutingModule { }
