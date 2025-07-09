<template>
  <v-container fluid fill-height class="login-container">
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card class="elevation-12">
          <v-toolbar color="primary" dark flat>
            <v-toolbar-title>Iniciar Sesión - Administrador</v-toolbar-title>
          </v-toolbar>
          
          <v-card-text>
            <v-form @submit.prevent="login" ref="form">
              <v-text-field
                v-model="loginData.username"
                label="Usuario"
                name="username"
                prepend-icon="mdi-account"
                type="text"
                :rules="[rules.required]"
                required
              />

              <v-text-field
                v-model="loginData.password"
                label="Contraseña"
                name="password"
                prepend-icon="mdi-lock"
                type="password"
                :rules="[rules.required]"
                required
              />
            </v-form>
          </v-card-text>
          
          <v-card-actions>
            <v-spacer />
            <v-btn
              color="primary"
              :loading="loading"
              @click="login"
              :disabled="!loginData.username || !loginData.password"
            >
              Iniciar Sesión
            </v-btn>
          </v-card-actions>
        </v-card>

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
      </v-col>
    </v-row>
  </v-container>
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
}
</style> 