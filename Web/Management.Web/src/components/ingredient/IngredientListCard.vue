<template>
  <div class="list-card">

    <div class="card-image-container" :class="{ inactive: !item.isActive }">
      <img :src="item.imageUrl" :alt="item.name" />
    </div>

    <div class="card-content-container">
      <h2 class="name">{{ item.name }}</h2>
    </div>

    <footer class="card-footer">
      <p>Цена: {{ item.price }} ₽</p>
      <div class="actions">
        <button title="Редактировать" @click="update(item.id)">
          <i class="fas fa-pen"></i>
        </button>
        <button title="Копировать" @click="copy(item.id)">
          <i class="fas fa-copy"></i>
        </button>
      </div>
    </footer>

  </div>
</template>

<script setup lang="ts">
import {IngredientListItem} from "@/models/ingredient/ingredientListItem";

const props = defineProps<{
  item: IngredientListItem;
}>();

const emit = defineEmits<{
  (event: 'update', id: number): void;
  (event: 'copy', id: number): void;
}>();

function update(id: number) {
  emit('update', id);
}

function copy(id: number) {
  emit('copy', id);
}
</script>

<style scoped>
.name {
  margin: 0 0 0.3rem;
  font-size: 1.2rem;
}

.inactive {
  opacity: 0.7;
  filter: grayscale(80%);
}

.actions button {
  background-color: transparent;
  border: none;
  color: var(--secondary-text-color, #777);
  cursor: pointer;
  font-size: 1.1rem;
  transition: color 0.3s ease;
  user-select: none;
}

.actions button:hover {
  color: var(--primary-color, #e74c3c);
}

.card-footer {
  justify-content: space-between;
}
</style>