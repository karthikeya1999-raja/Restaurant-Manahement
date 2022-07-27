import { Dish } from "./dish.modal";

export interface Restaurant{
    id?: number,
    name: string,
    type: string,
    imgUrl: string,
    dishes?: Dish[]
}