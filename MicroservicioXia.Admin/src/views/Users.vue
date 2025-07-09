<template>
  <div class="users-container">
    <v-row>
      <v-col cols="12">
        <div class="d-flex justify-space-between align-center mb-8">
          <div class="d-flex align-center">
            <v-icon size="40" color="primary" class="mr-4">mdi-account-group</v-icon>
            <h1 class="text-h3 font-weight-light">Gestión de Usuarios</h1>
          </div>
          <v-btn
            color="primary"
            size="large"
            @click="showCreateDialog = true"
            class="create-btn"
          >
            <v-icon left size="24">mdi-account-plus</v-icon>
            Crear Usuario
          </v-btn>
        </div>
      </v-col>
    </v-row>

    <!-- Filtros y búsqueda -->
    <v-row class="mb-6">
      <v-col cols="12" lg="4">
        <v-text-field
          v-model="search"
          label="Buscar usuarios..."
          prepend-icon="mdi-magnify"
          clearable
          variant="outlined"
          size="large"
        ></v-text-field>
      </v-col>
      <v-col cols="12" lg="3">
        <v-select
          v-model="roleFilter"
          :items="roleOptions"
          label="Filtrar por rol"
          clearable
          variant="outlined"
          size="large"
        ></v-select>
      </v-col>
      <v-col cols="12" lg="3">
        <v-select
          v-model="statusFilter"
          :items="statusOptions"
          label="Filtrar por estado"
          clearable
          variant="outlined"
          size="large"
        ></v-select>
      </v-col>
    </v-row>

    <!-- Tabla de usuarios -->
    <v-card elevation="3" class="users-table-card">
      <v-data-table
        :headers="headers"
        :items="filteredUsers"
        :loading="loading"
        :search="search"
        :items-per-page="15"
        class="elevation-0"
        density="comfortable"
      >
        <template v-slot:item.isActive="{ item }">
          <v-chip
            :color="item.isActive ? 'success' : 'error'"
            size="small"
            variant="flat"
          >
            {{ item.isActive ? 'Activo' : 'Inactivo' }}
          </v-chip>
        </template>

        <template v-slot:item.role="{ item }">
          <v-chip
            :color="item.role === 'Admin' ? 'warning' : 'info'"
            size="small"
            variant="flat"
          >
            {{ item.role }}
          </v-chip>
        </template>

        <template v-slot:item.createdAt="{ item }">
          {{ formatDate(item.createdAt) }}
        </template>

        <template v-slot:item.actions="{ item }">
          <div class="d-flex gap-2">
            <v-btn
              icon
              size="small"
              color="primary"
              variant="text"
              @click="editUser(item)"
              class="action-btn"
            >
              <v-icon>mdi-pencil</v-icon>
            </v-btn>
            <v-btn
              icon
              size="small"
              color="error"
              variant="text"
              @click="deleteUser(item)"
              class="action-btn"
            >
              <v-icon>mdi-delete</v-icon>
            </v-btn>
          </div>
        </template>
      </v-data-table>
    </v-card>

    <!-- Diálogo para crear/editar usuario -->
    <v-dialog v-model="showCreateDialog" max-width="700px">
      <v-card>
        <v-card-title class="pa-6">
          <span class="text-h5">{{ editingUser ? 'Editar Usuario' : 'Crear Usuario' }}</span>
        </v-card-title>

        <v-card-text class="pa-6">
          <v-form ref="form" v-model="valid">
            <v-row>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.username"
                  label="Usuario"
                  :rules="[rules.required]"
                  required
                  variant="outlined"
                  size="large"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.email"
                  label="Email"
                  type="email"
                  :rules="[rules.required, rules.email]"
                  required
                  variant="outlined"
                  size="large"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.firstName"
                  label="Nombre"
                  :rules="[rules.required]"
                  required
                  variant="outlined"
                  size="large"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.lastName"
                  label="Apellido"
                  :rules="[rules.required]"
                  required
                  variant="outlined"
                  size="large"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-text-field
                  v-model="userForm.password"
                  label="Contraseña"
                  type="password"
                  :rules="editingUser ? [] : [rules.required, rules.minLength]"
                  :required="!editingUser"
                  variant="outlined"
                  size="large"
                ></v-text-field>
              </v-col>
              <v-col cols="12" sm="6">
                <v-select
                  v-model="userForm.role"
                  :items="roleOptions"
                  label="Rol"
                  :rules="[rules.required]"
                  required
                  variant="outlined"
                  size="large"
                ></v-select>
              </v-col>
              <v-col cols="12">
                <v-switch
                  v-model="userForm.isActive"
                  label="Usuario Activo"
                  color="primary"
                ></v-switch>
              </v-col>
            </v-row>
          </v-form>
        </v-card-text>

        <v-card-actions class="pa-6 pt-0">
          <v-spacer></v-spacer>
          <v-btn
            color="grey"
            variant="text"
            size="large"
            @click="showCreateDialog = false"
          >
            Cancelar
          </v-btn>
          <v-btn
            color="primary"
            variant="elevated"
            size="large"
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
    <v-dialog v-model="showDeleteDialog" max-width="500px">
      <v-card>
        <v-card-title class="text-h5 pa-6">Confirmar Eliminación</v-card-title>
        <v-card-text class="pa-6">
          ¿Está seguro de que desea eliminar al usuario "{{ userToDelete?.username }}"?
          Esta acción no se puede deshacer.
        </v-card-text>
        <v-card-actions class="pa-6 pt-0">
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" size="large" @click="showDeleteDialog = false">
            Cancelar
          </v-btn>
          <v-btn color="error" variant="elevated" size="large" @click="confirmDelete" :loading="deleting">
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

<style scoped>
.users-container {
  padding: 20px;
  max-width: 1400px;
  margin: 0 auto;
}

.create-btn {
  border-radius: 12px;
  transition: all 0.3s ease;
}

.create-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15) !important;
}

.users-table-card {
  border-radius: 16px;
  overflow: hidden;
}

.action-btn {
  transition: all 0.2s ease;
}

.action-btn:hover {
  transform: scale(1.1);
}

.gap-2 {
  gap: 8px;
}

/* Mejoras para pantallas grandes */
@media (min-width: 1200px) {
  .users-container {
    padding: 40px;
  }
}

/* Responsive adjustments */
@media (max-width: 600px) {
  .users-container {
    padding: 16px;
  }
  
  .create-btn {
    font-size: 0.9rem;
  }
}
</style> 