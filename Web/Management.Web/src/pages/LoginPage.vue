<template>
  <div class="login-container">
    <pizzeriaLoader v-if="authStore.isLoading" />
    <form class="login-form">
      <h2>Вход</h2>

      <div class="login-content-container">
        <div class="form-group">
          <label class="label">Логин</label>
          <pizzeriaInput
              ref="loginInput"
              type="text"
              id="login"
              v-model="login"
              placeholder="Введите логин *"
              required
          />
        </div>

        <div class="form-group">
          <label class="label">Пароль</label>
          <pizzeriaInput
              ref="passwordInput"
              type="text"
              id="password"
              v-model="password"
              placeholder="Введите пароль *"
              required
          />
        </div>
      </div>
      <footer class="modal-actions">
        <button type="button" @click="register" class="secondary-standard-button">Регистрация</button>
        <button type="button" @click="submitLogin" class="standard-button">Войти</button>
      </footer>

    </form>
  </div>
</template>

<script setup lang="ts">
import {ref, watch} from 'vue'
import {useRouter} from "vue-router";
import {useErrorDialogStore} from "@/store/errorDialogStore";
import {useAuthStore} from "@/store/authStore";
import pizzeriaInput from "@/components/shared/PizzeriaInput.vue";
import pizzeriaLoader from "@/components/shared/PizzeriaLoader.vue";
import {StringUtils} from "@/stringUtils";
import {AuthErrors} from "@/models/auth/authErrors";

const login = ref('')
const password = ref('')
const loginInput = ref();
const passwordInput = ref();

const router = useRouter();
const authStore = useAuthStore();
const errorDialogStore = useErrorDialogStore();

watch(
    () => authStore.isSuccess,
    (newValue) => { if (newValue === false) handleError(); }
);

function handleError(){
  switch (authStore.error){
    case AuthErrors.InvalidPasswordOrLogin: onInvalidPasswordOrLogin(); break;
  }
}

function onInvalidPasswordOrLogin(){
  loginInput.value.triggerValidation(true);
  passwordInput.value.triggerValidation(true);

  errorDialogStore.show("Неверный логин или пароль", () => authStore.isSuccess = true)
}


async function submitLogin() {
  if (!validate()) return;
  await authStore.login(login.value, password.value);
}

function validate(): boolean {
  const invalid = {
    login: StringUtils.isNullOrEmpty(login.value),
    password: StringUtils.isNullOrEmpty(password.value)
  };

  loginInput.value.triggerValidation(invalid.login);
  passwordInput.value.triggerValidation(invalid.password);

  return !Object.values(invalid).some(v => v);
}

async function register(){
  await router.push('/register');
}

</script>

<style scoped>
.login-container {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: var(--secondary-background-color);
}

.login-form {
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

.login-form h2 {
  text-align: center;
}

.login-content-container {
  display: flex;
  flex-direction: column;
  padding-right: 10px;
  padding-left: 10px;
  padding-bottom: 20px;
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
