import {defineStore} from "pinia";
import {ref} from "vue";
import {useErrorDialogStore} from "@/store/errorDialogStore";
import ingredientApi from "@/api/ingredientApi";
import {IngredientListItem} from "@/models/ingredient/ingredientListItem";
import {Ingredient} from "@/models/ingredient/Ingredient";
import {IngredientCreateModel} from "@/models/ingredient/ingredientCreateModel";
import {IngredientUpdateModel} from "@/models/ingredient/ingredientUpdateModel";
import {ApiResponseError} from "@/models/apiResponseError";
import {IngredientErrors} from "@/models/ingredient/ingredientErrors";

export const useIngredientStore = defineStore('ingredientStore', () => {
    const isLoading = ref(false);
    const isSuccess = ref(false);
    const error = ref<IngredientErrors>()
    const items = ref<IngredientListItem[]>([]);

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

    function updateItem(ingredient: Ingredient): void {
        const index: number = items.value.findIndex(item => item.id === ingredient.id);
        if (index !== -1) {
            items.value[index] = mapIngredientToListItem(ingredient);
        } else {
            items.value.push(mapIngredientToListItem(ingredient));
        }
    }

    function mapIngredientToListItem(ingredient: Ingredient): IngredientListItem {
        return {
            id: ingredient.id,
            name: ingredient.name,
            imageUrl: ingredient.imageUrl,
            isActive: ingredient.isActive,
            price: ingredient.price
        };
    }

    async function buildCreateFrom(model: IngredientCreateModel) : Promise<FormData> {
            const formData: FormData = new FormData();

            formData.append('imageFile', model.imageFile);
            formData.append('name', model.name);
            formData.append('price', model.price.toString());
            formData.append('isActive', model.isActive.toString());

            return formData;
        }

    async function buildUpdateFrom(model: IngredientUpdateModel) : Promise<FormData> {
            const formData: FormData = new FormData();

            formData.append('id', model.id.toString());
            formData.append('name', model.name);
            formData.append('price', model.price.toString());
            formData.append('isActive', model.isActive.toString());
            formData.append('imageId', model.imageId.toString());
            if (model.imageFile) formData.append('imageFile', model.imageFile);
            if (model.imageUrl) formData.append('imageUrl', model.imageUrl);

            return formData;
        }

    function handleErrors(apiError: ApiResponseError): void {
        error.value = IngredientErrors.None;
        if (!apiError) {
            useErrorDialogStore().showServerError();
            return;
        }

        switch (apiError.type) {
            case 'VersionConflict': error.value = IngredientErrors.VersionConflict; break;
            default: useErrorDialogStore().showServerError();
        }
    }

    async function loadIngredients(): Promise<void> {
        await callApiWithLoading(async () => {
            const response = await ingredientApi.getIngredients();
            items.value = response.data.items;
        });
    }

    async function getCreateOptions(copyFromId?: number): Promise<Ingredient | null> {
        return await callApiWithLoading(async () => {
            const response = await ingredientApi.getCreateOptions({ copyFromId });
            if (response != null){
                return response.data.ingredient;
            }
            return null;
        });
    }

    async function getUpdateOptions(id: number): Promise<Ingredient | null> {
        return await callApiWithLoading(async () => {
            const response = await ingredientApi.getUpdateOptions({ id });
            if (response != null){
                return response.data.ingredient;
            }
            return null;
        });
    }

    async function createIngredient(model: IngredientCreateModel): Promise<boolean> {
         return await callApiWithLoading(async () => {
            const form: FormData = await buildCreateFrom(model);
            const response = await ingredientApi.create(form)
            if (response != null){
                updateItem(response.data.ingredient);
                return true;
            }
             return false;
        }) as boolean;
    }

    async function updateIngredient(model: IngredientUpdateModel): Promise<boolean> {
        return await callApiWithLoading(async () => {
            const form: FormData = await buildUpdateFrom(model);
            const response = await ingredientApi.update(model.id, form)
            if (response != null){
                updateItem(response.data.ingredient);
                return true;
            }
            return false;
        }) as boolean;
    }

    return {
        isLoading,
        isSuccess,
        items,
        error,
        loadIngredients,
        getCreateOptions,
        getUpdateOptions,
        createIngredient,
        updateIngredient
    };
});