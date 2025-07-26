import {ApiRequest} from "@/models/apiRequest";
import {PizzaPrice} from "@/models/pizza/pizzaPrice";

export interface UpdatePizzaApiRequest extends ApiRequest{
    id: number;
    name: string;
    description: string;
    isActive: boolean;
    ingredientIds: number[];
    prices: PizzaPrice[];
}