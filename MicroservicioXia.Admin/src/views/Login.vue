<template>
  <div class="login-container">
    <v-container fluid fill-height>
      <v-row align="center" justify="center" no-gutters>
        <v-col cols="12" sm="10" md="8" lg="6" xl="4">
          <v-card class="login-card" elevation="12">
            <v-toolbar color="primary" dark flat>
              <v-toolbar-title class="text-h5 font-weight-bold">
                Panel de Administración
              </v-toolbar-title>
            </v-toolbar>
            
            <v-card-text class="pa-6">
              <div class="text-center mb-6">
                <v-icon size="64" color="primary" class="mb-4">mdi-shield-account</v-icon>
                <h2 class="text-h4 font-weight-light mb-2">Bienvenido</h2>
                <p class="text-body-1 text-medium-emphasis">Ingresa tus credenciales de administrador</p>
              </div>
              
              <v-form @submit.prevent="login" ref="form" class="mt-4">
                <v-text-field
                  v-model="loginData.username"
                  label="Usuario"
                  name="username"
                  prepend-inner-icon="mdi-account"
                  type="text"
                  variant="outlined"
                  :rules="[rules.required]"
                  required
                  class="mb-4"
                />

                <v-text-field
                  v-model="loginData.password"
                  label="Contraseña"
                  name="password"
                  prepend-inner-icon="mdi-lock"
                  type="password"
                  variant="outlined"
                  :rules="[rules.required]"
                  required
                  class="mb-6"
                />
              </v-form>
            </v-card-text>
            
            <v-card-actions class="pa-6 pt-0">
              <v-btn
                color="primary"
                size="large"
                block
                :loading="loading"
                @click="login"
                :disabled="!loginData.username || !loginData.password"
                class="text-body-1 font-weight-medium"
              >
                <v-icon left>mdi-login</v-icon>
                Iniciar Sesión
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-col>
      </v-row>
    </v-container>

    <!-- Snackbar para mensajes -->
    <v-snackbar
      v-model="snackbar.show"
      :color="snackbar.color"
      :timeout="4000"
      location="top"
    >
      <div class="d-flex align-center">
        <v-icon left :color="snackbar.color === 'error' ? 'white' : 'white'">
          {{ snackbar.color === 'error' ? 'mdi-alert-circle' : 'mdi-check-circle' }}
        </v-icon>
        <span class="ml-2">{{ snackbar.message }}</span>
      </div>
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
import { ref, reactive } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const router = useRouter()
const form = ref(null)
const loading = ref(false)

const loginData = reactive({
  username: '',
  password: ''
})

const snackbar = reactive({
  show: false,
  message: '',
  color: 'success'
})

const rules = {
  required: v => !!v || 'Este campo es requerido'
}

const showMessage = (message, color = 'success') => {
  snackbar.message = message
  snackbar.color = color
  snackbar.show = true
}

const login = async () => {
  if (!form.value.validate()) return

  loading.value = true
  
  try {
    const response = await axios.post('/users/admin/login', loginData)
    
    // Guardar token y datos del usuario
    localStorage.setItem('adminToken', response.data.token)
    localStorage.setItem('adminUser', JSON.stringify({
      username: response.data.username,
      role: response.data.role
    }))
    
    showMessage('Inicio de sesión exitoso')
    
    // Redirigir al dashboard
    setTimeout(() => {
      router.push('/dashboard')
    }, 1000)
    
  } catch (error) {
    console.error('Error en login:', error)
    
    if (error.response?.status === 401) {
      showMessage('Credenciales inválidas o usuario no es administrador', 'error')
    } else {
      showMessage('Error al conectar con el servidor', 'error')
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.login-container {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
}

.login-card {
  border-radius: 16px;
  overflow: hidden;
  max-width: 450px;
  width: 100%;
}

/* Responsive adjustments */
@media (max-width: 600px) {
  .login-card {
    margin: 16px;
    border-radius: 12px;
  }
  
  .v-card-text {
    padding: 24px !important;
  }
  
  .v-card-actions {
    padding: 0 24px 24px 24px !important;
  }
}

@media (max-width: 400px) {
  .login-card {
    margin: 8px;
  }
  
  .v-card-text {
    padding: 16px !important;
  }
  
  .v-card-actions {
    padding: 0 16px 16px 16px !important;
  }
}

/* Smooth transitions */
.v-card {
  transition: all 0.3s ease;
}

.v-btn {
  transition: all 0.2s ease;
}

/* Custom scrollbar for webkit browsers */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
}

::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #555;
}
</style> 