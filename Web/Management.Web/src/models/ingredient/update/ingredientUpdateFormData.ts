import {ApiRequest} from "@/models/apiRequest";

export interface IngredientUpdateFormData extends ApiRequest{
    id: number;
    name: string;
    isActive: boolean;
    price: number;
}