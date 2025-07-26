import {PizzaPrice} from "@/models/pizza/pizzaPrice";

export interface PizzaUpdateModel {
    id: number
    name: string;
    description: string;
    isActive: boolean;
    imageFile?: File;
    imageUrl?: string;
    imageId: number;
    ingredientIds: number[];
    prices: PizzaPrice[];
}