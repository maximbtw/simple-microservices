<template>
  <div class="image-selector-container" @click="triggerFileInput">
    <img :src="url" alt="" />
    <div class="image-selector-overlay"
         :class="{
           'input-error': isInvalid,
           'shake': shouldShake
         }"
         @focus="clearError">
      <i class="fas fa-camera"/>
    </div>
    <input
        type="file"
        ref="fileInput"
        @change="onFileChange"
        accept="image/*"
        hidden
    />
  </div>
</template>

<script setup lang="ts">
import {nextTick, ref, watch} from 'vue';

const props = withDefaults(defineProps<{
  imageFile?: File;
  imageDefaultUrl?: string;
  id: string;
}>(), {
  imageFile: undefined,
  imageDefaultUrl: ''
});

const fileInput = ref<HTMLInputElement | null>(null);

const imageFile = defineModel<File | undefined>('imageFile');

const url = ref(props.imageDefaultUrl)

const isInvalid = ref(false);
const shouldShake = ref(false);

watch(
    () => props.imageDefaultUrl,
    (newUrl) => {
      if (!imageFile.value) {
        url.value = newUrl;
      }
    },
    { immediate: true }
);

function triggerFileInput() {
  clearError();
  fileInput.value?.click();
}

function onFileChange(event: Event) {
  const target = event.target as HTMLInputElement;
  const file = target.files?.[0];
  if (!file) return;

  const reader = new FileReader();
  reader.onload = () => {
    imageFile.value = file;
    url.value = reader.result as string;
  };
  reader.readAsDataURL(file);
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
.image-selector-container {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 25px;
  user-select: none;
  width: fit-content;
  cursor: pointer;
}

.image-selector-container img {
  width: 100%;
  max-width: 400px;
  border-radius: 8px;
  object-fit: cover;
  transition: opacity 0.3s ease;
}

.image-selector-overlay {
  position: absolute;
  top: 25px;
  left: 25px;
  right: 25px;
  bottom: 25px;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 4rem;
  background-color: rgba(0, 0, 0, 0);
  border-radius: 8px;
  transition: background-color 0.3s ease;
  pointer-events: none;
}

.image-selector-overlay i {
  opacity: 0.1;
  transition: opacity 0.3s ease, color 0.3s ease;
  color: var(--primary-color);
}

.image-selector-container:hover .image-selector-overlay i {
  opacity: 1;
}

.image-selector-container:hover img {
  opacity: 0.4;
}

.image-selector-container:hover .image-selector-overlay {
  background-color: rgba(0, 0, 0, 0.005);
  pointer-events: auto;
}

.image-selector-overlay.input-error i {
  opacity: 1 !important;
  color: var(--validation-error-color);
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
