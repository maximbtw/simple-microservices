import {ApiRequest} from "@/models/apiRequest";

export interface IngredientCreateFormData extends ApiRequest{
    name: string;
    imageId: number;
    isActive: boolean;
    price: number;
}