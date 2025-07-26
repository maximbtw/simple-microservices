<template>
  <div class="image-selector-overlay">
    <div class="spinner"></div>
  </div>
</template>

<script setup lang="ts">

</script>

<style scoped>
.image-selector-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.4);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
  backdrop-filter: blur(1px);
}

.spinner {
  width: 64px;
  height: 64px;
  border: 6px solid var(--background-color);
  border-top: 6px solid var(--primary-color);
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>