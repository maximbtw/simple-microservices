import { defineStore } from 'pinia';
import {ref} from "vue";
import {useErrorDialogStore} from "@/store/errorDialogStore";
import router from "@/router/router";
import authApi from "@/api/authApi";
import {LoginApiRequest} from "@/models/auth/login/loginApiRequest";
import {AccountUser} from "@/models/accountUser/accountUser";
import {RegisterApiRequest} from "@/models/auth/register/registerApiRequest";
import {AuthErrors} from "@/models/auth/authErrors";
import {ApiResponseError} from "@/models/apiResponseError";


export const useAuthStore = defineStore('authStore', () => {
    const isLoading = ref(false);
    const isSuccess = ref(true);
    const error = ref<AuthErrors>()

    async function callApiWithLoading<T>(action: () => Promise<T>): Promise<T | null> {
        isLoading.value = true;
        try {
            return await action();
        }
        catch (e: any) {
            if (e.response?.status === 409) {
                handleErrors(e.response.data);
                isSuccess.value = false;
                return null;
            }
                isSuccess.value = false;
                throw e;
        }
        finally {
            isLoading.value = false;
        }
    }

    function setAuth(token: string, login: string): void {
        localStorage.setItem('token', token)
        localStorage.setItem('login', login)
    }

    async function logout(): Promise<void> {
        localStorage.removeItem('token')
        localStorage.removeItem('login')
        await router.push('/login')
    }

    function getLogin(): string {
        return localStorage.getItem('login') as string;
    }

    function handleErrors(apiError: ApiResponseError): void {
        error.value = AuthErrors.None;
        if (!apiError) {
            useErrorDialogStore().showServerError();
            return;
        }

        switch (apiError.type) {
            case 'InvalidPasswordOrLogin': error.value = AuthErrors.InvalidPasswordOrLogin; break;
            case 'UserWithSameEmailExists': error.value = AuthErrors.UserWithSameEmailExists; break;
            case 'UserWithSameLoginExists': error.value = AuthErrors.UserWithSameLoginExists; break;
            default: useErrorDialogStore().showServerError();
        }
    }

    async function login(login: string, password: string): Promise<void> {
        await callApiWithLoading(async () => {
            const request: LoginApiRequest = {login: login, password: password}
            const response = await authApi.login(request)
            if (response != null){
                setAuth(response.data.token, response.data.accountUser.login);
                isSuccess.value = true;
                await router.push('/')
            }
        });
    }

    async function register(user: AccountUser): Promise<void> {
        await callApiWithLoading(async () => {
            const request: RegisterApiRequest = {
                accountId: user.accountId,
                login: user.login,
                password: user.password,
                email: user.email,
            }
            const response = await authApi.register(request)
            if (response != null){
                isSuccess.value = true;
                await router.push('/login')
            }
        });
    }

    return {
        isLoading,
        isSuccess,
        error,
        getLogin,
        login,
        logout,
        register
    };
});