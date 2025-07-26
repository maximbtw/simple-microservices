<template>
  <div class="list-main-container">
    <div class="filter-menu-container">
      <pizzeriaSearchBar
          v-model="searchQuery"
          placeholder="Поиск..."
          class="pizzeria-search-bar"
      />
      <button type="button" class="add-button" @click="add">
        <i class="fas fa-plus"></i>
      </button>
    </div>

    <div class="list-container">
      <card v-for="item in filteredItems"
            :key="item.id"
            :item="item"
            @update="update"
            @copy="copy"
      />
    </div>

    <pizzaForm
        v-if="modalOptions.visible"
        :pizzaId="modalOptions.pizzaId"
        :isEdit="modalOptions.editMode"
        @close="closeEditForm"
    />
  </div>
</template>

<script setup lang="ts">
import {ref, computed, watch, onMounted, reactive} from "vue";
import { useRoute, useRouter } from "vue-router";
import { usePizzaStore } from "@/store/pizzaStore";

import card from "@/components/pizza/PizzaListCard.vue";
import pizzaForm from "@/components/pizza/PizzaEditModal.vue";
import pizzeriaSearchBar from "@/components/shared/PizzeriaSearchBar.vue";


const searchQuery = ref('');

const modalOptions = reactive<{
  visible: boolean;
  editMode: boolean;
  pizzaId: number | undefined;
}>({
  visible: false,
  editMode: false,
  pizzaId: undefined
});

const route = useRoute();
const router = useRouter();

const pizzaStore = usePizzaStore();

const filteredItems = computed(() => {
  const query = searchQuery.value.trim().toLowerCase();

  const sortedItems = pizzaStore.items.slice().sort((a, b) => {
    // Сначала по активности (активные выше)
    if (a.isActive !== b.isActive) {
      return a.isActive ? -1 : 1;
    }
    // Потом по имени (в алфавитном порядке)
    return a.name.localeCompare(b.name);
  });

  if (!query) return sortedItems;

  return sortedItems.filter(item =>
      item.name.toLowerCase().includes(query)
  );
});

onMounted( async () => {
  await pizzaStore.loadPizzas()
});

watch(route, handleRouteChange, { immediate: true });

function handleRouteChange() {
  const { name, params, query } = route;

  if (name === 'CreatePizza') {
    modalOptions.editMode = false;
    modalOptions.pizzaId = query.copyId ? Number(query.copyId) : undefined;
    modalOptions.visible = true;
  } else if (name === 'EditPizza') {
    modalOptions.pizzaId = params.id ? Number(params.id) : undefined;
    modalOptions.editMode = true;
    modalOptions.visible = true;
  } else {
    modalOptions.visible = false;
  }
}

async function add() {
  await router.push({ name: 'CreatePizza' });
}

async function copy(id: number) {
  await router.push({ name: 'CreatePizza', query: { copyId: id } });
}

async function update(id: number) {
  await router.push({ name: 'EditPizza', params: { id } });
}

function closeEditForm() {
  router.push('/');
}
</script>

<style scoped>

.list-main-container{
  display: flex;
  flex-direction: column;
  gap: 20px;
}

.filter-menu-container{
  display: flex;
  align-items: center;
  gap: 20px;
}

.pizzeria-search-bar{
  max-width: 650px;
  min-width: 650px;
}

.list-container {
  display: grid;
  grid-template-columns: repeat(4, minmax(250px, 1fr));
  gap: 30px;
  justify-content: start;
}

.add-button {
  background: none;
  margin: 0;
  cursor: pointer;
  color: var(--text-color);
  font-size: 1.25rem;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  border: none;
  height: 10px;
}

.add-button i {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.add-button:hover {
  color: var(--secondary-color);
}

</style>
