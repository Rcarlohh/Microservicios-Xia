<template>
  <div>
    <v-row>
      <v-col cols="12">
        <h1 class="text-h4 mb-4">Dashboard de Administración</h1>
      </v-col>
    </v-row>

    <!-- Tarjetas de estadísticas -->
    <v-row>
      <v-col cols="12" sm="6" md="3">
        <v-card class="mx-auto" max-width="400">
          <v-card-text>
            <div class="text-h4 font-weight-bold text-primary">
              {{ stats.totalUsers }}
            </div>
            <div class="text-subtitle-1">Total de Usuarios</div>
          </v-card-text>
          <v-card-actions>
            <v-btn
              color="primary"
              text
              @click="$router.push('/users')"
            >
              Ver Usuarios
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card class="mx-auto" max-width="400">
          <v-card-text>
            <div class="text-h4 font-weight-bold text-success">
              {{ stats.activeUsers }}
            </div>
            <div class="text-subtitle-1">Usuarios Activos</div>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card class="mx-auto" max-width="400">
          <v-card-text>
            <div class="text-h4 font-weight-bold text-info">
              {{ stats.totalQuestions }}
            </div>
            <div class="text-subtitle-1">Preguntas de Seguridad</div>
          </v-card-text>
          <v-card-actions>
            <v-btn
              color="info"
              text
              @click="$router.push('/security-questions')"
            >
              Ver Preguntas
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card class="mx-auto" max-width="400">
          <v-card-text>
            <div class="text-h4 font-weight-bold text-warning">
              {{ stats.activeQuestions }}
            </div>
            <div class="text-subtitle-1">Preguntas Activas</div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Gráficos y tablas -->
    <v-row class="mt-6">
      <v-col cols="12" md="6">
        <v-card>
          <v-card-title>
            <v-icon left>mdi-account-group</v-icon>
            Usuarios Recientes
          </v-card-title>
          <v-card-text>
            <v-data-table
              :headers="userHeaders"
              :items="recentUsers"
              :loading="loading"
              :items-per-page="5"
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
              <template v-slot:item.createdAt="{ item }">
                {{ formatDate(item.createdAt) }}
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card>
          <v-card-title>
            <v-icon left>mdi-help-circle</v-icon>
            Preguntas de Seguridad
          </v-card-title>
          <v-card-text>
            <v-data-table
              :headers="questionHeaders"
              :items="securityQuestions"
              :loading="loading"
              :items-per-page="5"
              class="elevation-1"
            >
              <template v-slot:item.isActive="{ item }">
                <v-chip
                  :color="item.isActive ? 'success' : 'error'"
                  small
                >
                  {{ item.isActive ? 'Activa' : 'Inactiva' }}
                </v-chip>
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Acciones rápidas -->
    <v-row class="mt-6">
      <v-col cols="12">
        <v-card>
          <v-card-title>
            <v-icon left>mdi-lightning-bolt</v-icon>
            Acciones Rápidas
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="primary"
                  block
                  @click="$router.push('/users')"
                >
                  <v-icon left>mdi-account-plus</v-icon>
                  Crear Usuario
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="info"
                  block
                  @click="$router.push('/security-questions')"
                >
                  <v-icon left>mdi-help-plus</v-icon>
                  Crear Pregunta
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="success"
                  block
                  @click="refreshData"
                >
                  <v-icon left>mdi-refresh</v-icon>
                  Actualizar Datos
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="warning"
                  block
                  @click="exportData"
                >
                  <v-icon left>mdi-download</v-icon>
                  Exportar Datos
                </v-btn>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

<script setup>
import { ref, reactive, onMounted } from 'vue'
import axios from 'axios'

const loading = ref(false)
const stats = reactive({
  totalUsers: 0,
  activeUsers: 0,
  totalQuestions: 0,
  activeQuestions: 0
})

const recentUsers = ref([])
const securityQuestions = ref([])

const userHeaders = [
  { title: 'Usuario', key: 'username' },
  { title: 'Email', key: 'email' },
  { title: 'Estado', key: 'isActive' },
  { title: 'Fecha Creación', key: 'createdAt' }
]

const questionHeaders = [
  { title: 'ID', key: 'questionId' },
  { title: 'Pregunta', key: 'question' },
  { title: 'Estado', key: 'isActive' }
]

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString('es-ES')
}

const loadDashboardData = async () => {
  loading.value = true
  
  try {
    // Cargar usuarios
    const usersResponse = await axios.get('/users')
    const users = usersResponse.data
    
    // Cargar preguntas de seguridad
    const questionsResponse = await axios.get('/securityquestions')
    const questions = questionsResponse.data
    
    // Actualizar estadísticas
    stats.totalUsers = users.length
    stats.activeUsers = users.filter(u => u.isActive).length
    stats.totalQuestions = questions.length
    stats.activeQuestions = questions.filter(q => q.isActive).length
    
    // Usuarios recientes (últimos 5)
    recentUsers.value = users
      .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
      .slice(0, 5)
    
    // Preguntas de seguridad
    securityQuestions.value = questions.slice(0, 5)
    
  } catch (error) {
    console.error('Error cargando datos del dashboard:', error)
  } finally {
    loading.value = false
  }
}

const refreshData = () => {
  loadDashboardData()
}

const exportData = () => {
  // Implementar exportación de datos
  console.log('Exportar datos')
}

onMounted(() => {
  loadDashboardData()
})
</script> 