import axios from "@/plugins/axiosConfig";
import {GetAccountsApiResponse} from "@/models/account/getAccounts/getAccountsApiResponse";

export default class AccountApi {
    public static async getAccounts() {
        return await axios.get<GetAccountsApiResponse>('/accounts');
    }
}