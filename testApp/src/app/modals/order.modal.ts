export interface Order{
    orderId?: number,
    customerId: number,
    restaurantId: number,
    restaurantName: string,
    dishes: {
        dishId: number,
        dishName: string,
        dishCost: string,
        dishQuantity: number
    }[],
    orderedDate: string
}