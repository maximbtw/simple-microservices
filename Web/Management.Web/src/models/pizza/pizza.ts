import {PizzaPrice} from "@/models/pizza/pizzaPrice";
import {PizzaIngredient} from "@/models/pizza/pizzaIngredient";

export interface Pizza {
    id: number;
    name: string;
    imageUrl: string;
    imageId: number;
    description: string;
    isActive: boolean;
    prices: PizzaPrice[];
    ingredients: PizzaIngredient[]
}