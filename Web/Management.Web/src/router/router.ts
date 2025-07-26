import { createRouter, createWebHistory } from 'vue-router';
import HomePage from "@/pages/HomePage.vue";
import LoginPage from "@/pages/LoginPage.vue";
import PizzaEditModal from "@/components/pizza/PizzaEditModal.vue";
import IngredientEditModal from "@/components/ingredient/IngredientEditModal.vue";
import NotFoundPage from "@/pages/NotFoundPage.vue";
import RegisterPage from "@/pages/RegisterPage.vue";

const routes = [
    {
        path: '/',
        component: HomePage,
        children: [
            {
                path: 'pizzas/create',
                name: 'CreatePizza',
                component: PizzaEditModal,
                meta: { isModal: true }
            },
            {
                path: 'pizzas/edit/:id',
                name: 'EditPizza',
                component: PizzaEditModal,
                props: true,
                meta: { isModal: true }
            },
            {
                path: 'ingredients/create',
                name: 'CreateIngredient',
                component: IngredientEditModal,
                meta: { isModal: true }
            },
            {
                path: 'ingredients/edit/:id',
                name: 'EditIngredient',
                component: IngredientEditModal,
                props: true,
                meta: { isModal: true }
            }
        ]
    },
    {
        path: '/login',
        component: LoginPage
    },
    {
        path: '/register',
        component: RegisterPage
    },
    {
        path: '/:pathMatch(.*)*',
        name: 'NotFound',
        component: NotFoundPage,
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;