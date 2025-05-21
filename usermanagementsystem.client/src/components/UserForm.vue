<template>
  <div class="modal-backdrop">
    <div class="modal">
      <h3>{{ mode === 'add' ? 'Dodaj użytkownika' : 'Edytuj użytkownika' }}</h3>
      <form @submit.prevent="submitForm">
        <label>
          Imię:
          <input type="text" v-model="formData.name" @keyup="processNameChange" />
          <div v-if="errors.name" class="error">{{ errors.name }}</div>
        </label>
        <label>
          Email:
          <input type="text" v-model="formData.email" @keyup="processEmailChange" />
          <div v-if="errors.email" class="error">{{ errors.email }}</div>
        </label>
        <label>
          Avatar URL:
          <input type="text" v-model="formData.avatar" @keyup="processAvatarChange" />
          <div v-if="errors.avatar" class="error">{{ errors.avatar }}</div>
        </label>
        <div class="modal-buttons">
          <button type="submit" :disabled="!isValid">Zapisz</button>
          <button type="button" @click="$emit('cancel')">Anuluj</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import { computed, defineComponent, reactive, watch } from 'vue';
import type { PropType } from 'vue';

export default defineComponent({
  props: {
    user: {
      type: Object as PropType<{ id?: string; name: string; email: string; avatar?: string | null }>,
      required: true
    },
    mode: {
      type: String as PropType<'add' | 'edit'>,
      default: 'add'
    }
  },
  emits: ['submit', 'cancel'],
  setup(props, { emit }) {
    const formData = reactive({ ...props.user });
    const errors = reactive({
      name: '',
      email: '',
      avatar: '',
    });

    // Sync props -> reactive formData when props.user changes
    watch(
      () => props.user,
      (newUser) => {
        Object.assign(formData, newUser);
      },
      { deep: true, immediate: true }
    );

    function debounce<T extends (...args: any[]) => any>(func: T, wait: number): (...args: Parameters<T>) => void {
      let timeoutId: ReturnType<typeof setTimeout> | undefined;

      return function (...args: Parameters<T>) {
        if (timeoutId) {
          clearTimeout(timeoutId);
        }
        timeoutId = setTimeout(() => {
          func(...args);
        }, wait);
      };
    }

    const validateName = () => {
      if (!formData.name) {
        errors.name = 'Imię jest wymagane.';
      } else if (formData.name.length < 2 || formData.name.length > 100) {
        errors.name = 'Imię musi mieć między 2, a 100 znaków.';
      } else {
        errors.name = '';
      }
    };

    const debouncedValidateName = debounce(validateName, 300);
    function processNameChange() {
      debouncedValidateName();
    }

    const validateEmail = () => {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!formData.email) {
        errors.email = 'Email jest wymagany.';
      } else if (!emailRegex.test(formData.email)) {
        errors.email = 'Niepoprawy format adresu email.';
      } else {
        errors.email = '';
      }
    };

    const deboncedValidateEmail = debounce(validateEmail, 300);
    function processEmailChange() {
      deboncedValidateEmail();
    }

    const validateAvatar = () => {
      if (formData.avatar) {
        try {
          new URL(formData.avatar);
          errors.avatar = '';
        } catch {
          errors.avatar = 'Avarar musi być poprawnym adresem URL.';
        }
      } else {
        errors.avatar = '';
      }
    };

    const deboncedValidateAvatar = debounce(validateAvatar, 300);
    function processAvatarChange() {
      deboncedValidateAvatar();
    }

    const validateAll = () => {
      validateName();
      validateEmail();
      validateAvatar();
    };

    const isValid = computed(() => {
      return !errors.name && !errors.email && !errors.avatar;
    });

    const submitForm = () => {
      validateAll();
      if (isValid.value) {
        emit('submit', { ...formData });
      }
    };

    return {
      formData,
      errors,
      isValid,
      validateName,
      validateEmail,
      validateAvatar,
      processNameChange,
      processEmailChange,
      processAvatarChange,
      submitForm,
    };
  }
});
</script>

<style scoped>
.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal {
  background: var(--color-background);
  color: var(--color-text);
  padding: 2rem;
  border-radius: 8px;
  min-width: 300px;
}

.modal label {
  display: block;
  margin-bottom: 1rem;
}

.modal input {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid var(--color-border);
  border-radius: 4px;
  background-color: var(--color-background-soft);
  color: var(--color-text);
  transition: border-color 0.3s ease, box-shadow 0.3s ease;
}

.modal input:focus {
  outline: none;
  border-color: var(--vt-c-indigo);
  box-shadow: 0 0 5px var(--vt-c-indigo);
  background-color: var(--color-background);
  color: var(--color-text);
}

.modal-buttons {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
}

.error {
  color: red;
  font-size: 0.85em;
}

.modal-buttons button{
  width: 25%;
  height: 2rem;
  background-color: #1f2a38;
  color: var(--vt-c-text-dark-2);
  border-radius: 10%;
  border: none;
  cursor: pointer;
}

.modal-buttons button:disabled{
  background-color: var(--color-border);
  color: var(--color-text);
  cursor: default;
}
</style>
