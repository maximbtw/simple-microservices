<template>
  <form class="modal-form">
    <div class="modal-wrapper">

      <div class="modal-container">
        <div class="ingredient-selector-modal-content">

          <div>
            <h3 class="ingredient-selector-modal-title">Доступные</h3>
            <div class="ingredients-list-container">
              <div class="ingredients-check-item-container"
                   v-for="ingredient in ingredients.filter(x => !selectedIds.includes(x.id))"
                   :key="ingredient.id"
                    @click="toggleSelection(ingredient.id)"
                   :class="{ checked: availableSelectedIds.includes(ingredient.id) }">
                <div class="ingredient-item">
                  <img :src="ingredient.imageUrl" alt="" class="ingredient-image" />
                  <span class="ingredient-name">{{ ingredient.name }}</span>
                </div>
                <span class="checkmark" v-if="availableSelectedIds.includes(ingredient.id)">
                  <i class="fas fa-check" v-if="availableSelectedIds.includes(ingredient.id)"></i>
                </span>
              </div>
            </div>
          </div>

          <button type="button" class="icon-arrow-button" @click="chooseItems">
            <i class="fas fa-arrow-right"></i>
          </button>

          <div>
            <h3 class="ingredient-selector-modal-title">Выбранные</h3>
            <div class="ingredients-list-container">
              <div class="ingredients-check-item-container"
                   v-for="ingredient in ingredients.filter(x => selectedIds.includes(x.id))"
                   :key="ingredient.id">
                <div class="ingredient-item">
                  <img :src="ingredient.imageUrl" alt="" class="ingredient-image" />
                  <span class="ingredient-name">{{ ingredient.name }}</span>
                </div>
                <button type="button" class="remove-selected-item-button" @click="removeSelectedItem(ingredient.id)">
                  <i class="fas fa-times"></i>
                </button>
              </div>
            </div>
          </div>

        </div>

        <footer class="modal-actions">
          <button type="button" @click="add" class="standard-button">Добавить</button>
        </footer>
      </div>
      <button type="button" @click="closeModal" class="modal-close-button">
        <i class="fas fa-times"></i>
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">

import {ref} from "vue";
import {PizzaIngredient} from "@/models/pizza/pizzaIngredient";

const props = defineProps<{
  ingredients: PizzaIngredient[]
}>();

const selectedIds = ref<number[]>([]);
const availableSelectedIds = ref<number[]>([]);

const emit = defineEmits<{
  (e: 'add', value: number[]): void
  (e: 'close'): void
}>()


function closeModal() {
  emit("close");
}

function add() {
    emit("add", selectedIds.value);
}

function toggleSelection(id: number) {
  const includes = availableSelectedIds.value.includes(id);
  if (includes){
    availableSelectedIds.value = availableSelectedIds.value.filter(item => item !== id);
  }
  else{
    availableSelectedIds.value.push(id);
  }
}

function removeSelectedItem(id: number) {
  selectedIds.value = selectedIds.value.filter(item => item !== id);
}

function chooseItems(){
  availableSelectedIds.value.forEach((id) => {
    selectedIds.value.push(id);
  });

  availableSelectedIds.value = [];
}

</script>

<style scoped>

.modal-container{
  gap: 10px;
}

.ingredient-selector-modal-content{
  display: flex;
  align-items: center;
  user-select: none;
}

.ingredients-list-container{
  display: flex;
  flex-direction: column;
  min-width: 300px;
  min-height: 300px;
  border-radius: 25px;
  padding: 10px;
  border: 2px solid var(--border-color);
}

.ingredients-check-item-container{
  display: flex;
  justify-content: space-between;
  border-bottom: 1px solid var(--border-color);
  padding-bottom: 4px;
  user-select: none;
}
.ingredients-check-item-container:hover {
  color: var(--primary-color);
}
.ingredients-check-item-container.checked {
  color: var(--secondary-color);
}

.ingredient-selector-modal-title{
  display: flex;
  justify-content: center;
}

.ingredient-item {
  display: flex;
  align-items: center;
  cursor: pointer;
}

.ingredient-image {
  width: 16px;
  height: 16px;
  object-fit: cover;
  margin: 0 10px;
  border-radius: 4px;
  user-select: none;
}

.modal-actions {
  display: flex;
  justify-content: end;
  align-items: end;
  margin-top: auto;
}

.icon-arrow-button {
  background: none;
  padding-left: 10px;
  padding-right: 10px;
  padding-top: 20px;
  margin: 0;
  cursor: pointer;
  color: var(--text-color);
  font-size: 1.1rem;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border: none;
  height: 10px;
}

.icon-arrow-button i {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.icon-arrow-button:hover {
  color: var(--secondary-color);
}

.remove-selected-item-button {
  background: none;
  margin: 0;
  cursor: pointer;
  border: none;
  font-size: 1.1rem;
  color: var(--primary-color);
}
.remove-selected-item-button:hover {
  color: var(--secondary-color);
}

</style>