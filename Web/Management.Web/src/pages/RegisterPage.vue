<template>
  <div class="register-container">
    <pizzeriaLoader v-if="isLoading" />
    <form class="register-form">
      <h2>Регистрация</h2>

      <div class="register-content-container">
        <div class="form-group">
          <label class="label">Пиццерия</label>
          <pizzeriaSelector
              ref="accountSelector"
              v-model="model.accountId"
              :items="accountStore.accounts"
              placeholder="Выберите аккаунт *"
              item-title="name"
              item-value="id"
          />
        </div>

        <div class="form-group">
          <label class="label">Логин</label>
          <pizzeriaInput
              ref="loginInput"
              type="text"
              id="login"
              v-model="model.login"
              placeholder="Введите логин *"
              required
          />
        </div>

        <div class="form-group">
          <label class="label">Почта</label>
          <pizzeriaInput
              ref="emailInput"
              type="text"
              id="email"
              v-model="model.email"
              placeholder="Введите почту *"
              required
          />
        </div>

        <div class="form-group">
          <label class="label">Пароль</label>
          <pizzeriaInput
              ref="passwordInput"
              type="text"
              id="password"
              v-model="model.password"
              placeholder="Введите пароль *"
              required
          />
        </div>
      </div>
      <footer class="modal-actions">
        <button type="button" @click="enter" class="secondary-standard-button">Войти</button>
        <button type="button" @click="submitRegister" class="standard-button">Регистрация</button>
      </footer>

    </form>
  </div>
</template>

<script setup lang="ts">
import {computed, onMounted, reactive, ref, watch} from "vue";
import {useRouter} from "vue-router";
import {useAccountStore} from "@/store/accountStore";
import {useConfirmationDialogStore} from "@/store/confirmationDialogStore";
import {useAuthStore} from "@/store/authStore";
import PizzeriaLoader from "@/components/shared/PizzeriaLoader.vue";
import {StringUtils} from "@/stringUtils";
import {AccountUser} from "@/models/accountUser/accountUser";
import {AuthErrors} from "@/models/auth/authErrors";
import PizzeriaInput from "@/components/shared/PizzeriaInput.vue";
import pizzeriaSelector from "@/components/shared/PizzeriaSelector.vue";

const router = useRouter();
const accountStore = useAccountStore();
const authStore = useAuthStore();
const confirmationDialogStore = useConfirmationDialogStore();

const model = reactive<AccountUser>( {
  id: 0,
  login: '',
  email: '',
  password: '',
  accountId: 0,
});

const loginInput = ref();
const emailInput = ref();
const passwordInput = ref();
const accountSelector = ref();

const isLoading = computed(() => {
  return accountStore.isLoading || authStore.isLoading;
});

onMounted( async () => {
   await accountStore.loadAccounts();
});

watch(
    () => authStore.isSuccess,
    (newValue) => { if (newValue === false) handleError(); }
);

function handleError(){
  switch (authStore.error){
    case AuthErrors.UserWithSameLoginExists: onUserWithSameLoginExists(); break;
    case AuthErrors.UserWithSameEmailExists: onUserWithSameEmailExists(); break;
  }
}

function onUserWithSameLoginExists(){
  loginInput.value.triggerValidation(true);
  confirmationDialogStore.show(
      "Пользователь с таким логином уже существует",
      () => authStore.isSuccess = true,
      () => authStore.isSuccess = true)
}

function onUserWithSameEmailExists() {
  emailInput.value.triggerValidation(true);
  confirmationDialogStore.show("Пользователь с такой почтой уже существует",
      () => authStore.isSuccess = true,
      () => authStore.isSuccess = true)
}

async function submitRegister() {
  if (!validate()) return;
  await authStore.register(model);
}

function validate(): boolean {
  const invalid = {
    login: StringUtils.isNullOrEmpty(model.login),
    password: StringUtils.isNullOrEmpty(model.password),
    email: StringUtils.isNullOrEmpty(model.email),
    account: model.accountId == 0
  };

  loginInput.value.triggerValidation(invalid.login);
  passwordInput.value.triggerValidation(invalid.password);
  emailInput.value.triggerValidation(invalid.email);
  accountSelector.value.triggerValidation(invalid.account);

  return !Object.values(invalid).some(v => v);
}

async function enter(){
  await router.push('/login')
}
</script>

<style scoped>
.register-container {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: var(--secondary-background-color);
}

.register-form {
  background: var(--background-color);
  padding-left: 2rem;
  padding-right: 2rem;
  padding-bottom: 2rem;
  border-radius: 15px;
  width: 320px;
  box-shadow: 0 4px 12px rgb(0 0 0 / 0.1);
  display: flex;
  flex-direction: column;
}

.register-form h2 {
  text-align: center;
}

.register-content-container {
  display: flex;
  flex-direction: column;
  padding-bottom: 20px;
  padding-right: 10px;
  padding-left: 10px;
  gap: 10px;
  max-height: 600px;
  overflow-y: auto;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 7px;
}

.modal-actions {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: auto;
  padding-right: 10px;
  padding-left: 10px;
}
</style>