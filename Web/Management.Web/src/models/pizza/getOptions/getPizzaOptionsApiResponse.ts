import {Pizza} from "@/models/pizza/pizza";
import {PizzaIngredient} from "@/models/pizza/pizzaIngredient";

export interface GetPizzaOptionsApiResponse{
    pizza: Pizza | null;
    availableIngredients: PizzaIngredient[];
}