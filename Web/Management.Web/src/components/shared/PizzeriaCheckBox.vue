<template>
  <div class="checkbox-wrapper" @click="toggle">
    <button
        class="checkbox-button"
        :class="{ checked: modelValue }"
        type="button">
      <i v-if="modelValue" class="fas fa-check"></i>
    </button>
    <span class="checkbox-label">{{ label }}</span>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  modelValue: boolean;
  label: string;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
}>();

function toggle() {
  emit("update:modelValue", !props.modelValue);
}
</script>

<style scoped>
.checkbox-wrapper {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  user-select: none;
}

.checkbox-button {
  width: 28px;
  height: 28px;
  border-radius: 5px;
  border: 2px solid var(--border-color);
  display: flex;
  align-items: center;
  justify-content: center;
  transition: all 0.2s ease;
  background-color: var(--background-color);
  cursor: pointer;
}
.checkbox-button.checked {
  border: 2px solid var(--primary-color);
  color: var(--text-color);
}


</style>
