import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddRestaurantComponent } from './add-restaurant/add-restaurant.component';
import { ListRestaurantComponent } from './list-restaurant/list-restaurant.component';

const routes: Routes = [
    {path: "admin", children: [
        {path: "", redirectTo: "list-restaurant", pathMatch: "full"},
        {path: "add-restaurant", component: AddRestaurantComponent},
        {path: "list-restaurant", component: ListRestaurantComponent},
        {path: "edit-restaurant/:id", component: AddRestaurantComponent}
    ]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
