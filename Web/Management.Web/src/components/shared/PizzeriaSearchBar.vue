<template>
  <div class="custom-search-bar">
    <input
        v-model="searchQuery"
        :placeholder="placeholder"
        class="custom-input-field"
        @input="onInput"
    />
    <button v-if="searchQuery" class="clear-button" @click="clearSearch">✕</button>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';

const props = defineProps<{
  modelValue: string
  placeholder?: string
}>();

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void
}>();

const searchQuery = ref(props.modelValue);

const onInput = () => {
  emit('update:modelValue', searchQuery.value);
};

const clearSearch = () => {
  searchQuery.value = '';
  onInput();
};
</script>

<style scoped>
.custom-search-bar {
  position: relative;
  display: flex;
  align-items: center;
}

.custom-input-field {
  padding: 0.65rem 0.65rem 0.65rem 1rem;
  font-size: 0.95rem;
  font-weight: normal;
  border: 2px solid var(--border-color);
  border-radius: 20px;
  width: 100%;
  box-sizing: border-box;
  transition: border-color 0.3s ease;
}
.custom-input-field:focus {
  border: 2px solid var(--primary-color);
  outline: none;
}
.custom-input-field::placeholder {
  color: var(--text-color);
  opacity: 0.5;
}

.clear-button {
  position: absolute;
  right: 0.5rem;
  background: transparent;
  border: none;
  font-size: 1rem;
  cursor: pointer;
  color: #888;
}
</style>