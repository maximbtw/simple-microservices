import {defineStore} from "pinia";
import {ref} from "vue";
import accountApi from "@/api/accountApi";
import {account} from "@/models/account/account";

export const useAccountStore = defineStore('accountStore', () => {
    const isLoading = ref(false);
    const accounts = ref<account[]>([]);

    async function callApiWithLoading<T>(action: () => Promise<T>): Promise<T | null> {
        isLoading.value = true;
        try {
            return await action();
        }
        finally {
            isLoading.value = false;
        }
    }

    async function loadAccounts(): Promise<void> {
        await callApiWithLoading(async (): Promise<void> => {
            const response = await accountApi.getAccounts()
            if (response != null) {
                accounts.value = response.data?.accounts;
            }
        });
    }

    return {
        isLoading,
        accounts,
        loadAccounts
    };
});