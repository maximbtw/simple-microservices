<template>
  <input
      :id="id"
      v-bind="$attrs"
      v-model="internalValue"
      :type="type"
      :placeholder="placeholder"
      class="custom-input-field"
      :class="{
            'input-error': isInvalid,
            'shake': shouldShake
          }"
      @input="$emit('update:modelValue', internalValue)"
      @focus="clearError"
  />
</template>

<script setup lang="ts">
import { ref, watch, nextTick } from 'vue';

const props = defineProps({
  modelValue: {
    type: [String, Number],
    default: ''
  },
  type: {
    type: String,
    default: 'text'
  },
  id: {
    type: String,
    required: true
  },
  placeholder: {
    type: String,
    required: false
  }
});

const emit = defineEmits(['update:modelValue']);

const internalValue = ref(props.modelValue);

const isInvalid = ref(false);
const shouldShake = ref(false);

watch(() => props.modelValue, (newVal) => {
  internalValue.value = newVal;
});

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
.custom-input-field {
  padding: 0.65rem;
  font-size: 0.95rem;
  font-weight: normal;
  border: 2px solid var(--border-color);
  border-radius: 5px;
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
