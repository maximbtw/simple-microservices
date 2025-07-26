<template>
  <form class="modal-form" @submit.prevent="onSubmit">
    <div class="modal-wrapper">

      <div class="modal-container">
        <div class="text-container">
          <svg
              class="error-icon"
              xmlns="http://www.w3.org/2000/svg"
              fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2"
              width="24" height="24" aria-hidden="true"
          >
            <path stroke-linecap="round" stroke-linejoin="round" d="M12 9v2m0 4h.01M21 12c0 4.97-4.03 9-9 9s-9-4.03-9-9 4.03-9 9-9 9 4.03 9 9z" />
          </svg>
          {{ message }}
        </div>

        <footer class="modal-actions">
          <button type="button" @click="onSubmit" class="standard-button">Подтвердить</button>
        </footer>
      </div>
    </div>
  </form>
</template>

<script setup lang="ts">

import {useErrorDialogStore} from "@/store/errorDialogStore";

const dialogStore = useErrorDialogStore();

const message = dialogStore.message;

function onSubmit() {
  dialogStore.close()
}

</script>

<style scoped>

.modal-container {
  border: 2px solid var(--validation-error-color);
}

.text-container {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px 10px 10px;
  min-width: 500px;
  max-width: 500px;
  min-height: 100px;
  border-bottom: 2px solid var(--border-color);
  font-size: 1.25rem;
  gap: 10px;

  word-wrap: break-word;
  white-space: normal;
  text-align: center;
}

.modal-actions{
  padding-top: 10px;
  display: flex;
  justify-content: end;
}

.error-icon{
  color: var(--validation-error-color);
}

</style>