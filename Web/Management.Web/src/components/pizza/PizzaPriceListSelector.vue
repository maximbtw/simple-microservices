<template>
  <div class="list-container">
    <div
        class="list-item-container"
        v-for="price in sortedPrices"
        :key="price.size"
        @click="editPrice(price.size)"
    >
      <i class="fas fa-pen list-item-container-icon"></i>
      <div class="list-item-container-text">
        <p>{{ price.size }} см.</p>
        <p>{{ price.price }} ₽</p>
      </div>
    </div>

    <div class="list-item-container" @click="addPrice">
      <i class="fas fa-plus"></i>
    </div>

    <pizzaPriceEditModal
        v-if="isEditModalVisible"
        v-model="selectedPrice"
        :isEdit="modalIsEdit"
        @close="closeEditModal"
        @save="updateOrCreatePrice"
        @delete="deletePrice"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { PizzaPrice } from "@/models/pizza/pizzaPrice";
import pizzaPriceEditModal from "@/components/pizza/PizzaPriceListSelectorEditModal.vue";

const props = defineProps<{
  modelValue: PizzaPrice[];
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: PizzaPrice[]): void;
}>();

const isEditModalVisible = ref(false);
const modalIsEdit = ref(false);

const selectedPrice = ref<PizzaPrice>();
const originalPrice = ref<PizzaPrice>();

const sortedPrices = computed(() =>
    props.modelValue.slice().sort((a, b) => a.size - b.size)
);

function editPrice(size: number) {
  const price = props.modelValue.find((x) => x.size === size);
  if (!price) return;

  selectedPrice.value = { ...price };
  originalPrice.value = price;
  modalIsEdit.value = true;
  openEditModal();
}

function addPrice() {
  selectedPrice.value = undefined;
  originalPrice.value = undefined;
  modalIsEdit.value = false;
  openEditModal();
}

function openEditModal() {
  isEditModalVisible.value = true;
}

function closeEditModal() {
  isEditModalVisible.value = false;
  selectedPrice.value = undefined;
  originalPrice.value = undefined;
}

function updateOrCreatePrice(price: PizzaPrice) {
  const newPrices = [...props.modelValue];

  if (!modalIsEdit.value) {
    newPrices.push(price);
  } else if (originalPrice.value) {
    const index = newPrices.findIndex((p) => p === originalPrice.value);
    if (index !== -1) {
      newPrices[index] = price;
    }
  }

  emit("update:modelValue", newPrices);
  closeEditModal();
}

function deletePrice(price: PizzaPrice) {
  const newPrices = props.modelValue.filter((x) => x.size !== price.size);
  emit("update:modelValue", newPrices);
  closeEditModal();
}
</script>

<style scoped>
.list-container {
  display: grid;
  gap: 10px;
  grid-template-columns: repeat(auto-fill, minmax(75px, 1fr));
}

.list-item-container {
  position: relative;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  text-align: center;
  border-radius: 25px;
  padding: 5px;
  min-height: 30px;
  min-width: 60px;
  max-height: 40px;
  user-select: none;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.2);
  border: 2px solid rgba(0, 0, 0, 0);
}

.list-item-container:hover {
  border: 2px solid var(--primary-color);
}

.list-item-container p {
  padding: 0;
  margin: 0;
  font-weight: normal;
  font-size: 0.75rem;
}

.list-item-container-icon {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  opacity: 0;
  pointer-events: none;
}

.list-item-container:hover .list-item-container-icon {
  opacity: 1;
}

.list-item-container:hover .list-item-container-text {
  opacity: 0;
}
</style>
