import {AccountUser} from "@/models/accountUser/accountUser";

export interface LoginApiResponse {
    token: string;
    accountUser: AccountUser;
}