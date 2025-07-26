<template>
  <pizzeriaLoader v-if="isLoading" />
  <header class="header">
    <div class="header-left">
      <a class="logo">
        <i class="fas fa-pizza-slice" style="color: var(--primary-color); font-size: 32px;"></i>
      </a>
      <nav class="navigation">
        <a href="#" @click.prevent="scrollTo('pizzas');" :class="{ active: activeSection === 'pizzas' }">Пиццы</a>
        <a href="#" @click.prevent="scrollTo('ingredients');" :class="{ active: activeSection === 'ingredients' }">Ингридиенты</a>
        <a href="#" @click.prevent="scrollTo('extra-items');" :class="{ active: activeSection === 'extra-items' }">Закуски</a>
      </nav>
    </div>
    <div class="header-right">
      <a href="#" class="label-username">{{userLogin}}</a>
      <button class="standard-button" @click="logout">Выйти</button>
    </div>
  </header>

  <main>
    <div class="page-container">

      <section id="pizzas" >
        <h1 class="modal-title">Пиццы</h1>
        <pizzaList/>
      </section>

      <section id="ingredients">
        <h1 class="modal-title">Ингридиенты</h1>
        <ingredientList/>
      </section>

      <section id="extra-items">
        <h1 class="modal-title">Закуски</h1>
      </section>

    </div>
  </main>
</template>

<script setup lang="ts">
import {usePizzaStore} from "@/store/pizzaStore";
import {useIngredientStore} from "@/store/ingredientStore";
import {computed, onMounted, onUnmounted, ref} from "vue";
import {useAuthStore} from "@/store/authStore";
import pizzaList from "@/components/pizza/PizzaList.vue";
import PizzeriaLoader from "@/components/shared/PizzeriaLoader.vue";
import ingredientList from "@/components/ingredient/IngredientList.vue";

const pizzaStore = usePizzaStore();
const ingredientStore = useIngredientStore();
const authStore = useAuthStore();

const isLoading = computed(() => {
  return pizzaStore.isLoading || ingredientStore.isLoading || authStore.isLoading;
});

const userLogin = ref('');

const activeSection = ref<string | null>(null);
const headerOffset = 80;
const sectionIds = ['pizzas', 'ingredients', 'extra-items'];

onMounted( () => {
  userLogin.value = authStore.getLogin();

  window.addEventListener('scroll', onScroll);
  onScroll();
});

onUnmounted(() => {
  window.removeEventListener('scroll', onScroll);
});

function onScroll() {
  const scrollPosition = window.scrollY + headerOffset + 1;
  for (const sectionId of sectionIds) {
    const el = document.getElementById(sectionId);
    if (!el) continue;

    const top = el.offsetTop;
    const bottom = top + el.offsetHeight;

    if (scrollPosition >= top && scrollPosition < bottom) {
      activeSection.value = sectionId;
      return;
    }
  }

  activeSection.value = null;
}

function scrollTo(sectionId: string): void {
  const el = document.getElementById(sectionId);
  if (el) {
    el.scrollIntoView({ behavior: 'smooth' });
  }
}

async function logout(): Promise<void> {
  await authStore.logout();
}

</script>

<style scoped>
.header {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  box-sizing: border-box;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 200px;
  background-color: rgba(255, 255, 255, 0.95);
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  z-index: 1000;
}

.header-left {
  display: flex;
  gap: 2rem;
  justify-content: space-between;
  align-items: center;
}

.header-right{
  display: flex;
  gap: 2rem;
  justify-content: space-between;
  align-items: center;
}

.label-username{
  color: inherit;
  text-decoration: none;
  font-weight: normal;
  transition: color 0.3s ease;
}
.label-username:hover{
  color: var(--primary-color);
}

.navigation a {
  color: inherit;
  font-size: 16px;
  text-decoration: none;
  margin-right: 10px;
  transition: color 0.3s ease;
}
.navigation a:hover {
  color: var(--secondary-color);
}

.logo{
  user-select: none;
}

.navigation a.active {
  color: var(--primary-color);
}

main {
  margin: 0 auto;
  padding-top: 80px;
  padding-left: 200px;
  padding-right: 200px;
}

.page-container{
  display: flex;
  flex-direction: column;
  gap: 50px;
}

</style>