<template>
  <form class="modal-form">
    <pizzeriaLoader v-if="store.isLoading" />

    <div class="modal-wrapper">
      <div class="modal-container">

        <h2 class="modal-title">{{ isEdit ? "Редактирование ингредиента" : "Создание ингредиента" }}</h2>
        <div class="modal-image">
          <pizzeriaImageSelector
              ref="imageSelector"
              class="image-selector"
              id="ingredientImageUrl"
              v-model:imageFile="model.imageFile"
              :imageDefaultUrl="model.imageUrl"
          />
        </div>

        <div class="modal-edit-content">
          <div class="modal-edit-group">
            <label class="label">Название</label>
            <pizzeriaInput
                ref="nameInput"
                type="text"
                id="name"
                v-model="model.name"
                placeholder="Введите название *"
                required
            />
          </div>

          <div class="modal-edit-group">
            <label class="label">Цена</label>
            <pizzeriaInput
                ref="priceInput"
                type="number"
                id="price"
                v-model="model.price"
                placeholder="Введите цену *"
                required
            />
          </div>

          <div class="modal-edit-group">
            <pizzeriaCheckBox
                id="isActive"
                v-model="model.isActive!"
                label="Активно"
            />
          </div>
          <div>
        </div>
        <footer class="modal-actions">
          <button type="button" @click="saveAndCloseForm" class="standard-button">Сохранить</button>
        </footer>
        </div>
      </div>
      <button type="button" @click="closeModal" class="modal-close-button">
        <i class="fas fa-times"></i>
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import {useIngredientStore} from "@/store/ingredientStore";
import {onMounted, onUnmounted, reactive, ref, watch} from "vue";
import {useErrorDialogStore} from "@/store/errorDialogStore";
import PizzeriaLoader from "@/components/shared/PizzeriaLoader.vue";
import PizzeriaImageSelector from "@/components/shared/PizzeriaImageSelector.vue";
import PizzeriaInput from "@/components/shared/PizzeriaInput.vue";
import PizzeriaCheckBox from "@/components/shared/PizzeriaCheckBox.vue";
import {StringUtils} from "@/stringUtils";
import {IngredientUpdateModel} from "@/models/ingredient/ingredientUpdateModel";
import {IngredientCreateModel} from "@/models/ingredient/ingredientCreateModel";
import {IngredientErrors} from "@/models/ingredient/ingredientErrors";

const props = defineProps<{
  ingredientId?: number;
  isEdit: boolean;
}>();

const nameInput = ref();
const priceInput = ref();
const imageSelector = ref();

const emit = defineEmits(["close"]);

const store = useIngredientStore();
const errorDialogStore = useErrorDialogStore();

const model = reactive({
  id: 0,
  name: '',
  imageUrl: '',
  imageId: 0,
  imageFile: undefined,
  isActive: true,
  price: 0
});

onMounted(async () => {
  document.body.classList.add("no-scroll");
  await setOptions();
});

onUnmounted(() => {
  document.body.classList.remove("no-scroll");
});

watch(
    () => store.isSuccess,
    (newValue) => { if (newValue === false) handleError(); }
);

function handleError(){
  switch (store.error){
    case IngredientErrors.VersionConflict: onVersionConflict(); break;
  }
}

function onVersionConflict(){
  errorDialogStore.show(
      "Конфликт версий. Кто-то изменил модель первей раз.",
      () => store.isSuccess = true);
}

function getUpdateModel(model: any): IngredientUpdateModel {
  return { ...model } as IngredientUpdateModel;
}

function getCreateModel(model: any): IngredientCreateModel {
  return { ...model } as IngredientCreateModel;
}

async function setOptions(): Promise<void> {
  if(props.isEdit){
    const options = await store.getUpdateOptions(props.ingredientId as number);
    Object.assign(model, options);
  }
  else{
    const options = await store.getCreateOptions(props.ingredientId);
    Object.assign(model, options);
  }
}

async function saveAndCloseForm() {
  if (!validate()) return;

  const ok = props.isEdit
      ? await store.updateIngredient(getUpdateModel(model))
      : await store.createIngredient(getCreateModel(model));

  if (ok) {
    closeModal();
  }
}

function validate(): boolean {
  const invalid = {
    name: StringUtils.isNullOrEmpty(model.name),
    image: !model.imageFile && StringUtils.isNullOrEmpty(model.imageUrl),
    price: model.price <= 0
  };

  nameInput.value?.triggerValidation(invalid.name);
  imageSelector.value?.triggerValidation(invalid.image);
  priceInput.value?.triggerValidation(invalid.price);

  return !Object.values(invalid).some(v => v);
}

function closeModal() {
  emit("close");
}

</script>

<style scoped>
.modal-container{
  min-width: 600px;
  gap: 30px;
}

.modal-title{
  text-align: center;
}

.modal-image{
  display: flex;
  justify-content: center;
}

.image-selector{
  min-height: 300px;
  max-height: 300px;
  width: 100%;
}

.modal-actions {
  display: flex;
  justify-content: end;
  align-items: center;
  margin-top: auto;
  padding-right: 10px;
}

.modal-edit-content{
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.modal-edit-group{
  display: flex;
  flex-direction: column;
  gap: 7px;
}
</style>