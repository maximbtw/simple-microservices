export interface IngredientUpdateModel {
    id: number;
    name: string;
    price: number;
    isActive: boolean;
    imageFile?: File;
    imageUrl?: string;
    imageId: number;
}