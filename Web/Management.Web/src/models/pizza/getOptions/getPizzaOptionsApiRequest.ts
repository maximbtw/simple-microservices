import {ApiRequest} from "@/models/apiRequest";

export interface GetPizzaOptionsApiRequest extends ApiRequest{
    pizzaId: number | null;
}