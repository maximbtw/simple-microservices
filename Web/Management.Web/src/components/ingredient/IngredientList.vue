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
      <ingredientListCard v-for="item in filteredItems"
            :key="item.id"
            :item="item"
            @update="update"
            @copy="copy"
      />
    </div>

    <ingredientEditModal
        v-if="modalOptions.visible"
        :ingredientId="modalOptions.ingredientId"
        :isEdit="modalOptions.editMode"
        @close="closeEditForm"
    />
  </div>
</template>

<script setup lang="ts">
import {computed, onMounted, reactive, ref, watch} from "vue";
import {useIngredientStore} from "@/store/ingredientStore";
import {useRoute, useRouter} from "vue-router";
import ingredientListCard from "@/components/ingredient/IngredientListCard.vue";
import PizzeriaSearchBar from "@/components/shared/PizzeriaSearchBar.vue";
import ingredientEditModal from "@/components/ingredient/IngredientEditModal.vue";

const modalOptions = reactive<{
  visible: boolean;
  editMode: boolean;
  ingredientId: number | undefined;
}>({
  visible: false,
  editMode: false,
  ingredientId: undefined
});
const searchQuery = ref('');

const ingredientStore = useIngredientStore();
const route = useRoute();
const router = useRouter();

const filteredItems = computed(() => {
  const query = searchQuery.value.trim().toLowerCase();

  const sortedItems = ingredientStore.items.slice().sort((a, b) => {
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
  await ingredientStore.loadIngredients()
});

watch(route, handleRouteChange, { immediate: true });

function handleRouteChange() {
  const { name, params, query } = route;

  if (name === 'CreateIngredient') {
    modalOptions.editMode = false;
    modalOptions.ingredientId = query.copyId ? Number(query.copyId) : undefined;
    modalOptions.visible = true;
  } else if (name === 'EditIngredient') {
    modalOptions.ingredientId = params.id ? Number(params.id) : undefined;
    modalOptions.editMode = true;
    modalOptions.visible = true;
  } else {
    modalOptions.visible = false;
  }
}

async function add() {
  await router.push({ name: 'CreateIngredient' });
}

async function copy(id: number) {
  await router.push({ name: 'CreateIngredient', query: { copyId: id } });
}

async function update(id: number) {
  await router.push({ name: 'EditIngredient', params: { id } });
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
  grid-template-columns: repeat(5, minmax(150px, 1fr));
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