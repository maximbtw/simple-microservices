import {ApiRequest} from "@/models/apiRequest";

export interface GetIngredientOptionsApiRequest extends ApiRequest{
    ingredientId: number | null;
}