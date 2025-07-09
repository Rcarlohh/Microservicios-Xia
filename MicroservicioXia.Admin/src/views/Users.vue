<template>
  <div>
    <v-row>
      <v-col cols="12">
        <div class="d-flex justify-space-between align-center">
          <h1 class="text-h4">Gestión de Usuarios</h1>
          <v-btn
            color="primary"
            @click="showCreateDialog = true"
          >
            <v-icon left>mdi-account-plus</v-icon>
            Crear Usuario
          </v-btn>
        </div>
      </v-col>
    </v-row>

    <!-- Filtros y búsqueda -->
    <v-row class="mb-4">
      <v-col cols="12" md="4">
        <v-text-field
          v-model="search"
          label="Buscar usuarios..."
          prepend-icon="mdi-magnify"
          clearable
        ></v-text-field>
      </v-col>
      <v-col cols="12" md="3">
        <v-select
          v-model="roleFilter"
          :items="roleOptions"
          label="Filtrar por rol"
          clearable
        ></v-select>
      </v-col>
      <v-col cols="12" md="3">
        <v-select
          v-model="statusFilter"
          :items="statusOptions"
          label="Filtrar por estado"
          clearable
        ></v-select>
      </v-col>
    </v-row>

    <!-- Tabla de usuarios -->
    <v-card>
      <v-data-table
        :headers="headers"
        :items="filteredUsers"
        :loading="loading"
        :search="search"
        :items-per-page="10"
        class="elevation-1"
      >
        <template v-slot:item.isActive="{ item }">
          <v-chip
            :color="item.isActive ? 'success' : 'error'"
            small
          >
            {{ item.isActive ? 'Activo' : 'Inactivo' }}
          </v-chip>
        </template>

        <template v-slot:item.role="{ item }">
          <v-chip
            :color="item.role === 'Admin' ? 'warning' : 'info'"
            small
          >
            {{ item.role }}
          </v-chip>
        </template>

        <template v-slot:item.createdAt="{ item }">
          {{ formatDate(item.createdAt) }}
        </template>

        <template v-slot:item.actions="{ item }">
          <v-btn
            icon
            small
            color="primary"
            @click="editUser(item)"
          >
            <v-icon>mdi-pencil</v-icon>
          </v-btn>
          <v-btn
            icon
            small
            color="error"
            @click="deleteUser(item)"
          >
            <v-icon>mdi-delete</v-icon>
          </v-btn>
        </template>
      </v-data-table>
    </v-card>

    <!-- Diálogo para crear/editar usuario -->
    <v-dialog v-model="showCreateDialog" max-width="600px">
      <v-card>
        <v-card-title>
          <span class="text-h5">{{ editingUser ? 'Editar Usuario' : 'Crear Usuario' }}</span>
        </v-card-title>

        <v-card-text>
          <v-form ref="form" v-model="valid">
            <v-row>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.username"
                  label="Usuario"
                  :rules="[rules.required]"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.email"
                  label="Email"
                  type="email"
                  :rules="[rules.required, rules.email]"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.firstName"
                  label="Nombre"
                  :rules="[rules.required]"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.lastName"
                  label="Apellido"
                  :rules="[rules.required]"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.password"
                  label="Contraseña"
                  type="password"
                  :rules="editingUser ? [] : [rules.required, rules.minLength]"
                  :required="!editingUser"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-select
                  v-model="userForm.role"
                  :items="roleOptions"
                  label="Rol"
                  :rules="[rules.required]"
                  required
                ></v-select>
              </v-col>
              <v-col cols="12">
                <v-switch
                  v-model="userForm.isActive"
                  label="Usuario Activo"
                ></v-switch>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="blue darken-1"
            text
            @click="showCreateDialog = false"
          >
            Cancelar
          </v-btn>
          <v-btn
            color="blue darken-1"
            text
            @click="saveUser"
            :loading="saving"
            :disabled="!valid"
          >
            {{ editingUser ? 'Actualizar' : 'Crear' }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Diálogo de confirmación para eliminar -->
    <v-dialog v-model="showDeleteDialog" max-width="400px">
      <v-card>
        <v-card-title class="text-h5">Confirmar Eliminación</v-card-title>
        <v-card-text>
          ¿Está seguro de que desea eliminar al usuario "{{ userToDelete?.username }}"?
          Esta acción no se puede deshacer.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="blue darken-1" text @click="showDeleteDialog = false">
            Cancelar
          </v-btn>
          <v-btn color="error" text @click="confirmDelete" :loading="deleting">
            Eliminar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Snackbar para mensajes -->
    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      :timeout="3000"
    >
      {{ snackbar.message }}
      <template v-slot:actions>
        <v-btn
          color="white"
          text
          @click="snackbar.show = false"
        >
          Cerrar
        </v-btn>
      </template>
    </v-snackbar>
  </div>
</template>

<script setup>
import { ref, reactive, computed, onMounted } from 'vue'
import axios from 'axios'

const loading = ref(false)
const saving = ref(false)
const deleting = ref(false)
const valid = ref(false)
const search = ref('')
const roleFilter = ref('')
const statusFilter = ref('')

const showCreateDialog = ref(false)
const showDeleteDialog = ref(false)
const editingUser = ref(null)
const userToDelete = ref(null)

const users = ref([])

const userForm = reactive({
  username: '',
  email: '',
  firstName: '',
  lastName: '',
  password: '',
  role: 'User',
  isActive: true
})

const snackbar = reactive({
  show: false,
  message: '',
  color: 'success'
})

const headers = [
  { title: 'Usuario', key: 'username' },
  { title: 'Email', key: 'email' },
  { title: 'Nombre', key: 'firstName' },
  { title: 'Apellido', key: 'lastName' },
  { title: 'Rol', key: 'role' },
  { title: 'Estado', key: 'isActive' },
  { title: 'Fecha Creación', key: 'createdAt' },
  { title: 'Acciones', key: 'actions', sortable: false }
]

const roleOptions = [
  { title: 'Usuario', value: 'User' },
  { title: 'Administrador', value: 'Admin' }
]

const statusOptions = [
  { title: 'Activo', value: true },
  { title: 'Inactivo', value: false }
]

const rules = {
  required: v => !!v || 'Este campo es requerido',
  email: v => /.+@.+\..+/.test(v) || 'Email debe ser válido',
  minLength: v => v.length >= 6 || 'Mínimo 6 caracteres'
}

const filteredUsers = computed(() => {
  let filtered = users.value

  if (roleFilter.value) {
    filtered = filtered.filter(user => user.role === roleFilter.value)
  }

  if (statusFilter.value !== '') {
    filtered = filtered.filter(user => user.isActive === statusFilter.value)
  }

  return filtered
})

const showMessage = (message, color = 'success') => {
  snackbar.message = message
  snackbar.color = color
  snackbar.show = true
}

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString('es-ES')
}

const loadUsers = async () => {
  loading.value = true
  try {
    const response = await axios.get('/users')
    users.value = response.data
  } catch (error) {
    console.error('Error cargando usuarios:', error)
    showMessage('Error al cargar usuarios', 'error')
  } finally {
    loading.value = false
  }
}

const resetForm = () => {
  Object.assign(userForm, {
    username: '',
    email: '',
    firstName: '',
    lastName: '',
    password: '',
    role: 'User',
    isActive: true
  })
  editingUser.value = null
}

const editUser = (user) => {
  editingUser.value = user
  Object.assign(userForm, {
    username: user.username,
    email: user.email,
    firstName: user.firstName,
    lastName: user.lastName,
    password: '',
    role: user.role,
    isActive: user.isActive
  })
  showCreateDialog.value = true
}

const saveUser = async () => {
  if (!valid.value) return

  saving.value = true
  try {
    if (editingUser.value) {
      // Actualizar usuario
      await axios.put(`/users/${editingUser.value.id}`, {
        firstName: userForm.firstName,
        lastName: userForm.lastName,
        email: userForm.email
      })
      
      // Actualizar rol si es necesario
      if (userForm.role !== editingUser.value.role) {
        await axios.put(`/users/${editingUser.value.id}/role`, {
          role: userForm.role
        })
      }
      
      showMessage('Usuario actualizado exitosamente')
    } else {
      // Crear usuario
      await axios.post('/users', userForm)
      showMessage('Usuario creado exitosamente')
    }
    
    showCreateDialog.value = false
    resetForm()
    loadUsers()
  } catch (error) {
    console.error('Error guardando usuario:', error)
    showMessage('Error al guardar usuario', 'error')
  } finally {
    saving.value = false
  }
}

const deleteUser = (user) => {
  userToDelete.value = user
  showDeleteDialog.value = true
}

const confirmDelete = async () => {
  deleting.value = true
  try {
    await axios.delete(`/users/${userToDelete.value.id}`)
    showMessage('Usuario eliminado exitosamente')
    showDeleteDialog.value = false
    loadUsers()
  } catch (error) {
    console.error('Error eliminando usuario:', error)
    showMessage('Error al eliminar usuario', 'error')
  } finally {
    deleting.value = false
  }
}

onMounted(() => {
  loadUsers()
})
</script> 