import axios from '@/plugins/axiosConfig'
import {RegisterApiResponse} from "@/models/auth/register/registerApiResponse";
import {RegisterApiRequest} from "@/models/auth/register/registerApiRequest";
import {LoginApiResponse} from "@/models/auth/login/loginApiResponse";
import {LoginApiRequest} from "@/models/auth/login/loginApiRequest";

export default class AuthApi {
    public static async login(request: LoginApiRequest){
        return await axios.post<LoginApiResponse>('/auth/login', request);
    }

    public static async register(request: RegisterApiRequest){
        return await axios.post<RegisterApiResponse>('/auth/register', request);
    }
}