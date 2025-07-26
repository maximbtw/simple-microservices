import {ApiRequest} from "@/models/apiRequest";
import {PizzaPrice} from "@/models/pizza/pizzaPrice";

export interface CreatePizzaApiRequest extends ApiRequest{
    name: string;
    imageId: number;
    description: string;
    isActive: boolean;
    ingredientIds: number[];
    prices: PizzaPrice[];
}