import {ApiRequest} from "@/models/apiRequest";

export interface LoginApiRequest extends ApiRequest{
    login: string;
    password: string;
}