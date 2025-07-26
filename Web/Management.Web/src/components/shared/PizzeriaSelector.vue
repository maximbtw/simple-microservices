<template>
  <select
      :value="modelValue"
      place
      @change="onChange"
      :disabled="disabled"
      :class="{
            'input-error': isInvalid,
            'shake': shouldShake
          }"
      @focus="clearError"
  >
    <option
        v-for="item in items"
        :key="item[itemValue]"
        :value="item[itemValue]"
    >
      {{ item[itemTitle] }}
    </option>
  </select>
</template>


<script setup lang="ts">
import { nextTick, ref } from 'vue'

interface SelectorItem {
  [key: string]: any
}

const props = defineProps<{
  items: SelectorItem[]
  modelValue: any
  itemTitle?: string
  itemValue?: string
  disabled?: boolean
  placeholder?: string
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: any): void
}>()

const isInvalid = ref(false);
const shouldShake = ref(false);

const itemTitle = props.itemTitle ?? 'title'
const itemValue = props.itemValue ?? 'value'

function onChange(event: Event) {
  const target = event.target as HTMLSelectElement
  const value = target.value
  emit('update:modelValue', value)
}

function triggerValidation(invalid: boolean) {
  if (invalid) {
    isInvalid.value = true;

    shouldShake.value = false;
    nextTick(() => {
      shouldShake.value = true;
      setTimeout(() => {
        shouldShake.value = false;
      }, 300);
    });
  } else {
    isInvalid.value = false;
  }
}

function clearError() {
  isInvalid.value = false;
  shouldShake.value = false;
}

defineExpose({
  triggerValidation
});
</script>

<style scoped>
select {
  padding: 0.65rem;
  font-size: 0.95rem;
  font-weight: normal;
  border: 2px solid var(--border-color);
  border-radius: 5px;
  width: 100%;
  box-sizing: border-box;
  transition: border-color 0.3s ease;
}
select:focus {
  border: 2px solid var(--primary-color);
  outline: none;
}

select::placeholder {
  color: var(--text-color);
  opacity: 0.5;
}

option {
  padding: 0.65rem;
  font-size: 0.95rem;
  color: var(--text-color);
  background-color: white;
}

.input-error {
  border: 2px solid var(--validation-error-color);
}

@keyframes shake {
  0%, 100% { transform: translateX(0); }
  20%, 60% { transform: translateX(-5px); }
  40%, 80% { transform: translateX(5px); }
}

.shake {
  animation: shake 0.3s ease;
}
</style>