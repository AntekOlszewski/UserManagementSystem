<template>
  <div class="user-component">
    <h1>Users</h1>

    <div v-if="loading" class="loading">
      Loading... Please refresh once the ASP.NET backend has started. See <a
        href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
    </div>

    <div v-if="users" class="content">
      <table>
        <thead>
          <tr>
            <th></th>
            <th>Name</th>
            <th>Email</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.id" :class="{ selected: selectedUser?.id === user.id }"
            @click="selectUser(user)" @dblclick="editUser">
            <td class="avatar">
              <img v-if="user.avatar" :src="user.avatar" />
              <img v-else
                :src="'https://as1.ftcdn.net/v2/jpg/03/46/83/96/1000_F_346839683_6nAPzbhpSkIpb8pmAwufkC7c5eD7wYws.jpg'"
                alt="avatar" />
            </td>
            <td>{{ user.name }}</td>
            <td>{{ user.email }}</td>
          </tr>
        </tbody>
      </table>

      <div class="buttons">
        <button @click="addUser">Dodaj</button>
        <button :disabled="!selectedUser" @click="editUser">Edytuj</button>
        <button :disabled="!selectedUser" @click="deleteUser">Usuń</button>
      </div>
    </div>
  </div>

  <UserForm v-if="isFormOpen" :user="formUser" :mode="formMode" @submit="handleFormSubmit"
    @cancel="isFormOpen = false" />
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import UserForm from './UserForm.vue';

type User = {
  id: string,
  name: string,
  email: string,
  avatar: string | null
};

type UserData = {
  name: string,
  email: string,
  avatar: string | null
};

interface Data {
  loading: boolean,
  users: User[] | null,
  selectedUser: null | User,
  isFormOpen: boolean,
  formMode: 'add' | 'edit',
  formUser: UserData
};

export default defineComponent({
  data(): Data {
    return {
      loading: false,
      users: null,
      selectedUser: null,
      isFormOpen: false,
      formMode: 'add',
      formUser: { name: '', email: '', avatar: '' }
    };
  },
  async created() {
    await this.fetchData();
  },
  watch: {
    '$route': 'fetchData'
  },
  components: { UserForm },
  methods: {
    async fetchData() {
      this.users = null;
      this.loading = true;

      var response = await fetch('/api/users/');
      if (response.ok) {
        this.users = await response.json();
        this.loading = false;
      }
    },
    selectUser(user: User) {
      this.selectedUser = user;
    },

    addUser() {
      this.formMode = 'add';
      this.formUser = { name: '', email: '', avatar: '' };
      this.isFormOpen = true;
    },
    editUser() {
      if (this.selectedUser) {
        this.formMode = 'edit';
        this.formUser = { ...this.selectedUser };
        this.isFormOpen = true;
      }
    },
    async handleFormSubmit(userData : UserData) {
      if(!userData.avatar)
        userData.avatar = null;
      let response;
      if (this.formMode === 'add') {
        response = await fetch('/api/users/', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(userData)
        });
      } else {
        response = await fetch(`/api/users/${this.selectedUser?.id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify(userData)
        });
      }

      if (response.ok) {
        this.isFormOpen = false;
        await this.fetchData();
      } else {
        const errorData = await response.json();
        alert(errorData.error);
      }
    },

    async deleteUser() {
      if (!this.selectedUser) return;

      const confirmed = confirm(`Czy na pewno chcesz usunąć użytkownika ${this.selectedUser.name}?`);
      if (!confirmed) return;

      const response = await fetch(`/api/users/${this.selectedUser.id}`, {
        method: 'DELETE',
      });

      if (response.ok) {
        this.selectedUser = null;
        await this.fetchData();
      } else {
        const errorData = await response.json();
        alert(errorData.error);
      }
    },
  },
});
</script>

<style scoped>
th {
  font-weight: bold;
}

th,
td {
  padding-left: 1rem;
  padding-right: 1rem;
}

.user-component {
  text-align: center;
}

table {
  margin-left: auto;
  margin-right: auto;
  border-collapse: collapse;
}

tbody tr:hover {
  background-color: var(--color-background-mute);
  cursor: pointer;
}

.avatar img {
  width: 50px;
  height: 50px;
  border-radius: 50%;
}

.selected {
  background-color: var(--color-background-mute);
}

.buttons {
  margin-top: 1rem;
  display: flex;
  gap: 1rem;
  justify-content: space-around;
}

.buttons button{
  width: 25%;
  height: 2rem;
  background-color: #1f2a38;
  color: var(--vt-c-text-dark-2);
  border-radius: 10%;
  border: none;
  cursor: pointer;
}

.buttons button:disabled{
  background-color: var(--color-border);
  color: var(--color-text);
  cursor: default;
}
</style>
