import {defineStore} from "pinia";
import {ref} from "vue";

export const useConfirmationDialogStore = defineStore('confirmationDialogStore', () => {
    const message = ref('');
    const visible = ref(false);

    let onConfirmCallback: (() => void) | null = null;
    let onCloseCallback: (() => void) | null = null;

    function show(msg: string, onConfirm?: () => void, onClose?: () => void) {
        message.value = msg;
        visible.value = true;
        onConfirmCallback = onConfirm || null;
        onCloseCallback = onClose || null;
    }

    function close() {
        visible.value = false;
        onCloseCallback?.();
        clearCallbacks();
    }

    function confirm() {
        visible.value = false;
        onConfirmCallback?.();
        clearCallbacks();
    }

    function clearCallbacks() {
        onConfirmCallback = null;
        onCloseCallback = null;
    }

    return {
        message,
        visible,
        show,
        close,
        confirm
    };
});