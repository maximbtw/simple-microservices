import axios, {AxiosInstance} from 'axios';
import router from "@/router/router";
import {useErrorDialogStore} from "@/store/errorDialogStore";

const instance: AxiosInstance = axios.create({
    baseURL: '/api',
})

instance.interceptors.request.use((config) => {
    const token: string | null = localStorage.getItem('token')
    if (token) {
        config.headers.Authorization = `Bearer ${token}`
    }
    return config
})

instance.interceptors.response.use(
    response => response,
    error => {
        const status = error.response?.status;

        if (status === 401) {
            router.push('/login');
        } else if (status === 500) {
            useErrorDialogStore().showServerError(() => window.location.reload());
        }else if (status === 403) {
            useErrorDialogStore().showAccessError(() => window.location.reload());
        }else if (status === 404) {
            router.push('/not-found');
        }

        return Promise.reject(error);
    }
);

export default instance