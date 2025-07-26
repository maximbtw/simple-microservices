<template>
  <div class="custom-input-container">
    <textarea
        :id="id"
        v-bind="$attrs"
        v-model="internalValue"
        :placeholder="placeholder"
        :rows="rows"
        :cols="cols"
        :maxlength="maxlength"
        class="custom-input-field"
        :class="{
            'input-error': isInvalid,
            'shake': shouldShake
          }"
        @input="$emit('update:modelValue', internalValue)"
        @focus="clearError"
    />
  </div>
</template>

<script setup lang="ts">
import {ref, watch, nextTick} from 'vue';

const props = defineProps({
  modelValue: {
    type: [String, Number],
    default: ''
  },
  id: {
    type: String,
    required: true
  },
  placeholder: {
    type: String,
    required: false
  },
  rows: {
    type: Number,
    default: 4
  },
  cols: {
    type: Number,
    default: 50
  },
  maxlength: {
    type: Number,
    default: 200
  }
});

const emit = defineEmits();

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
.custom-input-container {
  display: flex;
  flex-direction: column;
}

.custom-input-field {
  padding: 0.65rem;
  border: 2px solid var(--border-color);
  border-radius: 5px;
  width: 100%;
  box-sizing: border-box;
  transition: border-color 0.3s ease;
  resize: none;
  overflow: auto;
  font-size: 0.95rem;
  font-weight: normal;
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
