import axios from '@/plugins/axiosConfig'
import {GetPizzasApiResponse} from "@/models/pizza/getPizzas/getPizzasApiResponse";
import {GetPizzaOptionsApiResponse} from "@/models/pizza/getOptions/getPizzaOptionsApiResponse";
import {CreatePizzaApiResponse} from "@/models/pizza/create/createPizzaApiResponse";
import {UpdatePizzaApiResponse} from "@/models/pizza/update/updatePizzaApiResponse";


export default class PizzaApi {
    public static async getPizzas(){
        return await axios.get<GetPizzasApiResponse>('/pizzas');
    }

    public static async getCreateOptions(request: { copyFromId?: number }) {
        return await axios.get<GetPizzaOptionsApiResponse>('/pizzas/create-options', {params: request});
    }

    public static async getUpdateOptions(request: { id: number }) {
        return await axios.get<GetPizzaOptionsApiResponse>(`/pizzas/${request.id}/update-options`);
    }

    public static async create(formData: FormData){
        console.log([...formData.entries()]);
        return await axios.post<CreatePizzaApiResponse>('/pizzas', formData);
    }

    public static async update(id: number, formData: FormData){
        return await axios.put<UpdatePizzaApiResponse>(`/pizzas/${id}`, formData);
    }
}