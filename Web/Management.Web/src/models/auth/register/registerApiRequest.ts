import {ApiRequest} from "@/models/apiRequest";

export interface RegisterApiRequest extends ApiRequest {
    accountId: number
    login: string;
    password: string;
    email: string
}