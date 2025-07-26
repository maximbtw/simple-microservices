<template>
  <router-view />
  <pizzeriaErrorDialog v-if="errorDialogStore.visible"/>
  <pizzeriaConfirmationDialog v-if="confirmationDialogStore.visible"/>
</template>

<script setup lang="ts">

import {useErrorDialogStore} from "@/store/errorDialogStore";
import {useConfirmationDialogStore} from "@/store/confirmationDialogStore";
import pizzeriaErrorDialog from "@/components/shared/PizzeriaErrorDialog.vue";
import pizzeriaConfirmationDialog from "@/components/shared/PizzeriaConfirmationDialog.vue";

const errorDialogStore = useErrorDialogStore();
const confirmationDialogStore = useConfirmationDialogStore();

</script>

<style scoped>

</style>