import axios from "@/plugins/axiosConfig";
import {GetIngredientsApiResponse} from "@/models/ingredient/getIngredients/getIngredientsApiResponse";
import {GetIngredientOptionsApiResponse} from "@/models/ingredient/getOptions/getIngredientOptionsApiResponse";
import {CreateIngredientApiResponse} from "@/models/ingredient/create/createIngredientApiResponse";
import {UpdateIngredientApiResponse} from "@/models/ingredient/update/updateIngredientApiResponse";

export default class IngredientApi {
    public static async getIngredients(){
        return await axios.get<GetIngredientsApiResponse>('/ingredients');
    }

    public static async getCreateOptions(request: { copyFromId?: number }) {
        return await axios.get<GetIngredientOptionsApiResponse>('/ingredients/create-options', {
            params: request
        });
    }

    public static async getUpdateOptions(request: { id: number }) {
        return await axios.get<GetIngredientOptionsApiResponse>(`/ingredients/${request.id}/update-options`);
    }

    public static async create(formData: FormData){
        return await axios.post<CreateIngredientApiResponse>('/ingredients', formData);
    }

    public static async update(id: number, formData: FormData){
        return await axios.put<UpdateIngredientApiResponse>(`/ingredients/${id}`, formData);
    }
}