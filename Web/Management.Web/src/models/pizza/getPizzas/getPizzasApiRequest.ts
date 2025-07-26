import {ApiRequest} from "@/models/apiRequest";

export interface GetPizzasApiRequest extends ApiRequest{
    take: number | null;
    skip: number | null;
}