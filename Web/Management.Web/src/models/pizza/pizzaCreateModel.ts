import {PizzaPrice} from "@/models/pizza/pizzaPrice";

export interface PizzaCreateModel {
    name: string;
    imageFile: File;
    description: string;
    isActive: boolean;
    ingredientIds: number[];
    prices: PizzaPrice[];
}