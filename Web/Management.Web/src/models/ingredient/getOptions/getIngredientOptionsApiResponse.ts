import {Ingredient} from "@/models/ingredient/Ingredient";

export interface GetIngredientOptionsApiResponse{
    ingredient: Ingredient | null;
}