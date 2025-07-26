<template>
  <div class="list-card">

    <div class="card-image-container" :class="{ inactive: !item.isActive }">
      <img :src="item.imageUrl" :alt="item.name" />
    </div>

    <div class="card-content-container">
      <h2 class="name">{{ item.name }}</h2>
      <p class="description">{{ item.description }}</p>
    </div>

    <footer class="card-footer">
      <p>Цена: {{ item.minPrice }} – {{ item.maxPrice }} ₽</p>
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
import { PizzaListItem } from "@/models/pizza/pizzaListItem";

const props = defineProps<{
  item: PizzaListItem;
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

.description {
  margin: 0;
  font-size: 0.9rem;
  font-weight: normal;
  color: var(--text-color);
  display: -webkit-box;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
}

.actions button {
  background-color: transparent;
  border: none;
  color: var(--secondary-text-color);
  cursor: pointer;
  font-size: 1.1rem;
  transition: color 0.3s ease;
  user-select: none;
}

.actions button:hover {
  color: var(--primary-color);
}
</style>
