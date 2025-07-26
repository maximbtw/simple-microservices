<template>
  <form class="modal-form">
    <div class="modal-wrapper">

      <div class="modal-container">
        <h2 class="pizza-price-modal-title">{{ isEdit ? "Редактирование цены" : "Создание цены" }}</h2>
        <div class="pizza-price-modal-content">

          <div class="pizza-price-modal-edit-group">
            <label class="label" for="pizzaName">Размер</label>
            <pizzeriaInput
                ref="sizeInput"
                type="number"
                id="size"
                v-model="modelValue.size"
                placeholder="Введите размер *"
                required
            />
          </div>

          <div class="pizza-price-modal-edit-group">
            <label class="label" for="pizzaName">Цена</label>
            <pizzeriaInput
                ref="priceInput"
                type="number"
                id="price"
                v-model="modelValue.price"
                placeholder="Введите цену *"
                required
            />
          </div>

        </div>

        <footer class="modal-actions">
          <button type="button" @click="deletePrice" class="secondary-standard-button">Удалить</button>
          <button type="button" @click="savePrice" class="standard-button">Сохранить</button>
        </footer>
      </div>

      <button type="button" @click="closeModal" class="modal-close-button">
        <i class="fas fa-times"></i>
      </button>
    </div>

    <PizzeriaConfirmationModal
        v-if="isDeletionConfirmationModalVisible"
        :text="`Вы дейсвительно хотите удалить цену?`"
        @apply="confirmDeletion"
        @close="closeDeletionConfirmationModal"
    />
  </form>
</template>

<script setup lang="ts">

import {PizzaPrice} from "@/models/pizza/pizzaPrice";
import pizzeriaInput from "@/components/shared/PizzeriaInput.vue";
import PizzeriaConfirmationModal from "@/components/shared/PizzeriaConfirmationDialog.vue";
import {ref} from "vue";

const props = withDefaults(defineProps<{
  modelValue?: PizzaPrice;
  isEdit: boolean;
}>(), {
  modelValue: () => ({ size: 0, price: 0 }),
  isEdit: false
});

const emit = defineEmits<{
  (e: 'save', value: PizzaPrice): void
  (e: 'delete', value: PizzaPrice): void
  (e: 'close'): void
}>()

const isDeletionConfirmationModalVisible = ref(false);

const sizeInput = ref();
const priceInput = ref();

function closeDeletionConfirmationModal() {
  isDeletionConfirmationModalVisible.value = false;
}

function confirmDeletion() {
  emit("delete", props.modelValue);

  closeDeletionConfirmationModal();
}

function closeModal() {
  emit("close");
}

function deletePrice() {
  isDeletionConfirmationModalVisible.value = true;
}

function savePrice(){
  if (!validate()) {
    return;
  }
  emit("save", props.modelValue);
}

function validate(): boolean {
  const invalid = {
    price: props.modelValue.price <= 0,
    size: props.modelValue.size <= 0
  };

  priceInput.value?.triggerValidation(invalid.price);
  sizeInput.value?.triggerValidation(invalid.size);

  return !Object.values(invalid).some(v => v);
}

</script>

<style scoped>

.pizza-price-modal-content {
  display: flex;
  align-items: center;
  padding-bottom: 20px;
  gap: 20px;
}

.pizza-price-modal-title{
  text-align: center;
}

.pizza-price-modal-edit-group{
  display: flex;
  flex-direction: column;
  gap: 5px;
}

.modal-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
}
</style>