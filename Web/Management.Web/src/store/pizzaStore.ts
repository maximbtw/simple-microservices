import {defineStore} from "pinia";
import {ref} from "vue";
import {useErrorDialogStore} from "@/store/errorDialogStore";

import pizzaApi from "@/api/pizzaApi";
import {Pizza} from "@/models/pizza/pizza";
import {PizzaListItem} from "@/models/pizza/pizzaListItem";
import {PizzaCreateModel} from "@/models/pizza/pizzaCreateModel";
import {PizzaUpdateModel} from "@/models/pizza/pizzaUpdateModel";
import {GetPizzaOptionsApiResponse} from "@/models/pizza/getOptions/getPizzaOptionsApiResponse";
import {PizzaErrors} from "@/models/pizza/pizzaErrors";
import {ApiResponseError} from "@/models/apiResponseError";

export const usePizzaStore = defineStore('pizzaStore', () => {
    const isLoading = ref(false);
    const isSuccess = ref(false);
    const error = ref<PizzaErrors>()
    const items = ref<PizzaListItem[]>([]);

    async function callApiWithLoading<T>(action: () => Promise<T>): Promise<T | null> {
        isLoading.value = true;
        try {
            return await action();
        } catch (e: any) {
            if (e.response?.status === 409) {
                handleErrors(e.response.data);
            }
            isSuccess.value = false;
            return null;
        }
        finally {
            isLoading.value = false;
        }
    }

    function handleErrors(apiError: ApiResponseError): void {
        error.value = PizzaErrors.None;
        if (!apiError) {
            useErrorDialogStore().showServerError();
            return;
        }

        switch (apiError.type) {
            case 'VersionConflict': error.value = PizzaErrors.VersionConflict; break;
            default: useErrorDialogStore().showServerError();
        }
    }

    function updateItem(pizza: Pizza): void {
        const index: number = items.value.findIndex(item => item.id === pizza.id);
        if (index !== -1) {
            items.value[index] = mapPizzaToListItem(pizza);
        } else {
            items.value.push(mapPizzaToListItem(pizza));
        }
    }

    function mapPizzaToListItem(pizza: Pizza): PizzaListItem {
        const prices: number[] = pizza.prices.map(p => p.price);
        return {
            id: pizza.id,
            name: pizza.name,
            imageUrl: pizza.imageUrl,
            description: pizza.description,
            isActive: pizza.isActive,
            minPrice: Math.min(...prices),
            maxPrice: Math.max(...prices)
        };
    }

    async function buildCreateFrom(model: PizzaCreateModel) : Promise<FormData> {
        const formData: FormData = new FormData();

        formData.append('name', model.name);
        formData.append('imageFile', model.imageFile);
        formData.append('description', model.description);
        formData.append('isActive', model.isActive.toString());
        formData.append('ingredientIdsJson', JSON.stringify(model.ingredientIds));
        formData.append('prices', JSON.stringify(model.prices));

        return formData;
    }

    async function buildUpdateFrom(model: PizzaUpdateModel) : Promise<FormData> {
        const formData: FormData = new FormData();

        formData.append('id', model.id.toString());
        formData.append('name', model.name);
        formData.append('description', model.description);
        formData.append('isActive', model.isActive.toString());
        formData.append('imageId', model.imageId.toString());
        formData.append('ingredientIdsJson', JSON.stringify(model.ingredientIds));
        formData.append('prices', JSON.stringify(model.prices));
        if (model.imageFile) formData.append('ImageFile', model.imageFile);
        if (model.imageUrl) formData.append('ImageUrl', model.imageUrl);

        return formData;
    }

    async function getCreateOptions(copyFromId?: number): Promise<GetPizzaOptionsApiResponse | null> {
        return await callApiWithLoading(async () => {
            const response = await pizzaApi.getCreateOptions({ copyFromId });
            if (response != null){
                return response.data;
            }
            return null;
        });
    }

    async function getUpdateOptions(id: number): Promise<GetPizzaOptionsApiResponse | null> {
        return await callApiWithLoading(async () => {
            const response = await pizzaApi.getUpdateOptions({ id });
            if (response != null){
                return response.data;
            }
            return null;
        });
    }

    async function createPizza(model: PizzaCreateModel): Promise<boolean> {
        return await callApiWithLoading(async () => {
            const form: FormData = await buildCreateFrom(model);
            const response = await pizzaApi.create(form)
            if (response != null){
                updateItem(response.data.pizza);
                return true;
            }
            return false;
        }) as boolean;
    }

    async function updatePizza(model: PizzaUpdateModel): Promise<boolean> {
        return await callApiWithLoading(async () => {
            const form: FormData = await buildUpdateFrom(model);
            const response = await pizzaApi.update(model.id, form)
            if (response != null){
                updateItem(response.data.pizza);
                return true;
            }
            return false;
        }) as boolean;
    }

    async function loadPizzas(): Promise<void> {
        await callApiWithLoading(async () => {
            const response = await pizzaApi.getPizzas();
            if (response != null){
                items.value = response.data.items;
            }
        });
    }

    return {
        isLoading,
        isSuccess,
        error,
        items,
        getCreateOptions,
        getUpdateOptions,
        createPizza,
        updatePizza,
        loadPizzas
    };
});
