import {defineStore} from "pinia";
import {ref} from "vue";

export const useErrorDialogStore = defineStore('errorDialogStore', () => {
    const message = ref('');
    const visible = ref(false);

    let onCloseCallback: (() => void) | null = null;

    function show(msg: string, onClose?: () => void) {
        message.value = msg;
        visible.value = true;
        onCloseCallback = onClose || null;
    }

    function showServerError(onClose?: () => void) {
        message.value = 'Что-то пошло не так. Попробуйте позже..';
        visible.value = true;
        onCloseCallback = onClose || null;
    }

    function showAccessError(onClose?: () => void) {
        message.value = "Доступ запрещен. Обратитесь к администратору.";
        visible.value = true;
        onCloseCallback = onClose || null;
    }

    function close() {
        visible.value = false;
        onCloseCallback?.();
        clearCallbacks();
    }

    function clearCallbacks() {
        onCloseCallback = null;
    }

    return {
        message,
        visible,
        showServerError,
        showAccessError,
        show,
        close
    };
});