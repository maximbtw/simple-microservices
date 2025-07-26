<template>
  <div class="pizza-ingredients-list-container">
    <!-- Кнопки взаимодействия -->
    <div class="pizza-ingredient-list-actions">
      <button type="button" class="square-icon-btn" @click="openModal">
        <i class="fas fa-plus"></i>
      </button>
      <button type="button" class="square-icon-btn" @click="onDeleteSelectedItems">
        <i class="fas fa-trash"></i>
      </button>
    </div>

    <!-- Список ингредиентов -->
    <div class="list-container">
      <button
          type="button"
          class="list-item-container"
          v-for="ingredient in modelValue"
          :key="ingredient.id"
          :class="{ checked: selectedIds.includes(ingredient.id) }"
          @click="toggleSelection(ingredient.id)"
      >
        <span class="checkmark" v-if="selectedIds.includes(ingredient.id)">
          <i class="fas fa-check"></i>
        </span>
        <span class="image-selector-container">
          <img :src="ingredient.imageUrl" alt="" class="ingredient-image" />
        </span>
        <span class="ingredient-name">{{ ingredient.name }}</span>
      </button>
    </div>

    <pizzaIngredientListSelectorModal
        v-if="isModalVisible"
        :ingredients="availableIngredientsFiltered"
        @close="closeModal"
        @add="addIngredients"
    />
  </div>
</template>

<script setup lang="ts">
import {ref, computed} from "vue";
import pizzaIngredientListSelectorModal from "@/components/pizza/PizzaIngredientListSelectorModal.vue";
import {PizzaIngredient} from "@/models/pizza/pizzaIngredient";
import {useConfirmationDialogStore} from "@/store/confirmationDialogStore";

const props = defineProps<{
  modelValue: PizzaIngredient[];
  availableIngredients: PizzaIngredient[];
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: PizzaIngredient[]): void;
}>();

const confirmationDialogStore = useConfirmationDialogStore();

const isModalVisible = ref(false);
const selectedIds = ref<number[]>([]);

const availableIngredientsFiltered = computed(() =>
    props.availableIngredients.filter(ai => !props.modelValue.some(mi => mi.id === ai.id))
);

function toggleSelection(id: number) {
  if (selectedIds.value.includes(id)) {
    selectedIds.value = selectedIds.value.filter(x => x !== id);
  } else {
    selectedIds.value.push(id);
  }
}

function openModal() {
  isModalVisible.value = true;
}

function closeModal() {
  isModalVisible.value = false;
}

function onDeleteSelectedItems() {
  if (selectedIds.value.length > 0) {
    confirmationDialogStore.show(
        "Вы действительно хотите удалить выбранные ингредиенты?",
        () => {
          deleteSelectedItems()
        }
    )
  }
}

function deleteSelectedItems() {
  const newList = props.modelValue.filter(x => !selectedIds.value.includes(x.id));
  emit("update:modelValue", newList);
  selectedIds.value = [];
}

function addIngredients(newIds: number[]) {
  const newItems = props.availableIngredients.filter(x =>
      newIds.includes(x.id)
  );

  const updatedList = [...props.modelValue, ...newItems].filter(
      (value, index, self) =>
          index === self.findIndex(v => v.id === value.id)
  );

  emit("update:modelValue", updatedList);
  closeModal();
}
</script>


<style scoped>

.pizza-ingredients-list-container{
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.list-container {
  display: flex;
  flex-wrap: wrap;
  gap: 10px calc((100% - 3 * 115px) / 2);
}

.list-item-container{
  display: flex;
  flex-direction: column;
  position: relative;
  border-radius: 5px;
  padding: 5px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);
  align-items: center;
  user-select: none;
  background-color: var(--background-color);
  border: 2px solid rgba(0,0,0,0);
  min-height: 130px;
  gap: 10px;
  width: 115px;
}
.list-item-container:hover{
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}
.list-item-container.checked {
  border: 2px solid var(--primary-color);
}

.image-selector-container img {
  width: 100%;
  border-radius: 8px;
  object-fit: cover;
}

.pizza-ingredient-list-actions{
  display: flex;
  gap: 5px;
  user-select: none;
}

.square-icon-btn {
  width: 30px;
  height: 30px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border: 2px solid var(--border-color);
  background-color: white;
  cursor: pointer;
  border-radius: 4px;
  color: var(--secondary-text-color);
}
.square-icon-btn:hover {
  color: var(--text-color);
  border-color: var(--primary-color);
}

.checkmark {
  position: absolute;
  display: flex;
  align-items: center;
  justify-content: center;
  top: 5px;
  right: 5px;
  color: var(--primary-color);
  border-radius: 50%;
  font-size: 1.05rem;
}

</style>