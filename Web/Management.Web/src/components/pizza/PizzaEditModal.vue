<template>
    <form class="modal-form">
      <pizzeriaLoader v-if="store.isLoading" />
      <div class="modal-wrapper">

        <div class="modal-container">
          <h2 class="modal-title">{{ isEdit ? "Редактирование пиццы" : "Создание пиццы" }}</h2>
          <div class="modal-content">

            <pizzeriaImageSelector
                ref="imageSelector"
                class="image-selector"
                id="pizzaImageUrl"
                v-model:imageFile="model.imageFile"
                :imageDefaultUrl="model.imageUrl"
            />

            <div class="pizza-modal-right-container">
              <div class="register-content-container">

                <div class="modal-edit-group">
                  <label class="label">Название</label>
                  <pizzeriaInput
                      ref="nameInput"
                      type="text"
                      id="pizzaName"
                      v-model="model.name"
                      placeholder="Введите название *"
                      required
                  />
                </div>

                <div class="modal-edit-group">
                  <pizzeriaCheckBox
                      id="pizzaIsActive"
                      v-model="model.isActive!"
                      label="Активно"
                  />
                </div>

                <div class="modal-edit-group">
                  <label>Цены</label>
                  <pizzaPriceListSelector
                      id="pizzaPrices"
                      v-model="model.prices"
                  />
                </div>

                <div class="modal-edit-group">
                  <label>Описание</label>
                  <pizzeriaTextarea
                      ref="descriptionTextarea"
                      id="pizzaDescription"
                      v-model="model.description"
                      :rows="4"
                      placeholder="Введите описание *"
                      required
                  />
                </div>

                <div class="modal-edit-group">
                  <label>Инридиенты</label>
                  <pizzaIngredientListSelector
                      id="pizzaIngredients"
                      v-model="model.ingredients"
                      :availableIngredients="availableIngredients"
                  />
                </div>

              </div>
              <footer class="modal-actions">
                <button type="button" @click="saveAndCloseForm" class="standard-button">Сохранить</button>
              </footer>
            </div>
          </div>
        </div>

        <button type="button" @click="closeForm" class="modal-close-button">
          <i class="fas fa-times"></i>
        </button>
      </div>
    </form>
</template>

<script setup lang="ts">
import {onMounted, onUnmounted, reactive, ref, watch} from "vue";
import {usePizzaStore} from "@/store/pizzaStore";
import {useErrorDialogStore} from "@/store/errorDialogStore";
import pizzeriaInput from "@/components/shared/PizzeriaInput.vue";
import pizzeriaTextarea from "@/components/shared/PizzeriaTextarea.vue";
import pizzaPriceListSelector from "@/components/pizza/PizzaPriceListSelector.vue";
import pizzaIngredientListSelector from "@/components/pizza/PizzaIngredientListSelector.vue";
import pizzeriaCheckBox from "@/components/shared/PizzeriaCheckBox.vue";
import pizzeriaImageSelector from "@/components/shared/PizzeriaImageSelector.vue";
import PizzeriaLoader from "@/components/shared/PizzeriaLoader.vue";
import {StringUtils} from "@/stringUtils";
import PizzeriaImageSelector from "@/components/shared/PizzeriaImageSelector.vue";
import {PizzaUpdateModel} from "@/models/pizza/pizzaUpdateModel";
import {PizzaCreateModel} from "@/models/pizza/pizzaCreateModel";
import {PizzaErrors} from "@/models/pizza/pizzaErrors";

const props = defineProps<{
  pizzaId?: number;
  isEdit: boolean;
}>();

const model = reactive({
  id: 0,
  name: '',
  imageUrl: '',
  imageId: 0,
  imageFile: undefined,
  description: '',
  isActive: true,
  prices: [],
  ingredients: []
});

const availableIngredients = reactive([]);

const nameInput = ref();
const descriptionTextarea = ref();
const imageSelector = ref();

const store = usePizzaStore();
const errorDialogStore = useErrorDialogStore();

const emit = defineEmits(["close"]);

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
    case PizzaErrors.VersionConflict: onVersionConflict(); break;
  }
}

function onVersionConflict(){
  errorDialogStore.show(
      "Конфликт версий. Кто-то изменил модель первей раз.",
      () => store.isSuccess = true);
}

function getUpdateModel(model: any): PizzaUpdateModel {
  return {
    id: model.id,
    name: model.name,
    imageFile: model.imageFile,
    imageUrl: model.imageUrl,
    imageId: model.imageId,
    description: model.description,
    isActive: model.isActive,
    ingredientIds: model.ingredients.map((x: any) => x.id),
    prices: model.prices
  }
}

function getCreateModel(model: any): PizzaCreateModel {
  return {
    name: model.name,
    imageFile: model.imageFile,
    description: model.description,
    isActive: model.isActive,
    ingredientIds: model.ingredients.map((x: any) => x.id),
    prices: model.prices
  }
}

async function setOptions(): Promise<void> {
  if (props.isEdit){
    const options = await store.getUpdateOptions(props.pizzaId as number);
    if (options != null){
      Object.assign(model, options.pizza);
      Object.assign(availableIngredients, options.availableIngredients);
    }
  }
  else{
    const options = await store.getCreateOptions(props.pizzaId);
    if (options != null) {
      Object.assign(model, options.pizza);
      Object.assign(availableIngredients, options.availableIngredients);
    }
  }
}

async function saveAndCloseForm() {
  if (!validate()) return;

  const ok = props.isEdit
      ? await store.updatePizza(getUpdateModel(model))
      : await store.createPizza(getCreateModel(model));

  if (ok) {
    closeForm();
  }
}

function validate(): boolean {
  const invalid = {
    name: StringUtils.isNullOrEmpty(model.name),
    description: StringUtils.isNullOrEmpty(model.description),
    ingredients: model.ingredients.length === 0,
    prices: model.prices.length === 0,
    image: !model.imageFile && StringUtils.isNullOrEmpty(model.imageUrl)
  };

  nameInput.value?.triggerValidation(invalid.name);
  descriptionTextarea.value?.triggerValidation(invalid.description);
  imageSelector.value?.triggerValidation(invalid.image);

  if (invalid.ingredients) alert("Нужно указать хоть один ингредиент!");
  if (invalid.prices) alert("Нужно указать хоть одну цену!");

  return !Object.values(invalid).some(v => v);
}

function closeForm() {
  emit("close");
}

</script>

<style scoped>
.modal-title{
  text-align: center;
}

.modal-content {
  display: flex;
  gap: 20px;
}

.pizza-modal-right-container{
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  min-width: 400px;
  max-width: 400px;
  min-height: 610px;
  max-height: 610px;
  gap: 10px;
}

.register-content-container {
  display: flex;
  flex-direction: column;
  padding-right: 10px;
  padding-left: 10px;
  padding-bottom: 10px;
  gap: 10px;
  max-height: 600px;
  overflow-y: auto;
}

.modal-edit-group{
  display: flex;
  flex-direction: column;
  gap: 7px;
}

.modal-actions {
  display: flex;
  justify-content: end;
  align-items: center;
  margin-top: auto;
  padding-right: 10px;
}

.image-selector {
  min-width: 400px;
}

</style>