<template>
  <form class="modal-form" @submit.prevent="onSubmit">
    <div class="modal-wrapper">

      <div class="modal-container">
        <div class="text-container">{{message}}</div>

        <footer class="modal-actions">
          <button type="button" @click="onSubmit" class="standard-button">Подтвердить</button>
        </footer>
      </div>

      <button type="button" @click="onClose" class="modal-close-button">
        <i class="fas fa-times"></i>
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">

import {useConfirmationDialogStore} from "@/store/confirmationDialogStore";

const dialogStore = useConfirmationDialogStore();

const message = dialogStore.message;

function onSubmit() {
  dialogStore.confirm();
}

function onClose() {
  dialogStore.close();
}

</script>

<style scoped>

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

  word-wrap: break-word;
  white-space: normal;
  text-align: center;
}

.modal-actions{
  padding-top: 10px;
  display: flex;
  justify-content: end;
}

</style>