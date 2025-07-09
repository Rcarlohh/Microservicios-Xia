<template>
  <div class="dashboard-container">
    <v-row>
      <v-col cols="12">
        <div class="d-flex align-center mb-8">
          <v-icon size="40" color="primary" class="mr-4">mdi-view-dashboard</v-icon>
          <h1 class="text-h3 font-weight-light">Dashboard de Administración</h1>
        </div>
      </v-col>
    </v-row>

    <!-- Tarjetas de estadísticas -->
    <v-row class="mb-8">
      <v-col cols="12" sm="6" lg="3">
        <v-card class="mx-auto stats-card" elevation="3">
          <v-card-text class="text-center pa-8">
            <v-icon size="56" color="primary" class="mb-4">mdi-account-group</v-icon>
            <div class="text-h2 font-weight-bold text-primary mb-3">
              {{ stats.totalUsers }}
            </div>
            <div class="text-h6 text-medium-emphasis">Total de Usuarios</div>
          </v-card-text>
          <v-card-actions class="pa-6 pt-0">
            <v-btn
              color="primary"
              variant="text"
              block
              size="large"
              @click="$router.push('/users')"
            >
              Ver Usuarios
              <v-icon right>mdi-arrow-right</v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" lg="3">
        <v-card class="mx-auto stats-card" elevation="3">
          <v-card-text class="text-center pa-8">
            <v-icon size="56" color="success" class="mb-4">mdi-account-check</v-icon>
            <div class="text-h2 font-weight-bold text-success mb-3">
              {{ stats.activeUsers }}
            </div>
            <div class="text-h6 text-medium-emphasis">Usuarios Activos</div>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" lg="3">
        <v-card class="mx-auto stats-card" elevation="3">
          <v-card-text class="text-center pa-8">
            <v-icon size="56" color="info" class="mb-4">mdi-help-circle</v-icon>
            <div class="text-h2 font-weight-bold text-info mb-3">
              {{ stats.totalQuestions }}
            </div>
            <div class="text-h6 text-medium-emphasis">Preguntas de Seguridad</div>
          </v-card-text>
          <v-card-actions class="pa-6 pt-0">
            <v-btn
              color="info"
              variant="text"
              block
              size="large"
              @click="$router.push('/security-questions')"
            >
              Ver Preguntas
              <v-icon right>mdi-arrow-right</v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" lg="3">
        <v-card class="mx-auto stats-card" elevation="3">
          <v-card-text class="text-center pa-8">
            <v-icon size="56" color="warning" class="mb-4">mdi-help-circle-outline</v-icon>
            <div class="text-h2 font-weight-bold text-warning mb-3">
              {{ stats.activeQuestions }}
            </div>
            <div class="text-h6 text-medium-emphasis">Preguntas Activas</div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Gráficos y tablas -->
    <v-row class="mb-8">
      <v-col cols="12" xl="6">
        <v-card elevation="3" class="h-100">
          <v-card-title class="d-flex align-center pa-6">
            <v-icon left color="primary" size="28">mdi-account-group</v-icon>
            <span class="text-h5">Usuarios Recientes</span>
          </v-card-title>
          <v-card-text class="pa-0">
            <v-data-table
              :headers="userHeaders"
              :items="recentUsers"
              :loading="loading"
              :items-per-page="5"
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
              <template v-slot:item.createdAt="{ item }">
                {{ formatDate(item.createdAt) }}
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" xl="6">
        <v-card elevation="3" class="h-100">
          <v-card-title class="d-flex align-center pa-6">
            <v-icon left color="info" size="28">mdi-help-circle</v-icon>
            <span class="text-h5">Preguntas de Seguridad</span>
          </v-card-title>
          <v-card-text class="pa-0">
            <v-data-table
              :headers="questionHeaders"
              :items="securityQuestions"
              :loading="loading"
              :items-per-page="5"
              class="elevation-0"
              density="comfortable"
            >
              <template v-slot:item.isActive="{ item }">
                <v-chip
                  :color="item.isActive ? 'success' : 'error'"
                  size="small"
                  variant="flat"
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
    <v-row>
      <v-col cols="12">
        <v-card elevation="3">
          <v-card-title class="d-flex align-center pa-6">
            <v-icon left color="warning" size="28">mdi-lightning-bolt</v-icon>
            <span class="text-h5">Acciones Rápidas</span>
          </v-card-title>
          <v-card-text class="pa-6">
            <v-row>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="primary"
                  variant="elevated"
                  block
                  size="x-large"
                  @click="$router.push('/users')"
                  class="mb-3 action-btn"
                >
                  <v-icon left size="24">mdi-account-plus</v-icon>
                  Crear Usuario
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="info"
                  variant="elevated"
                  block
                  size="x-large"
                  @click="$router.push('/security-questions')"
                  class="mb-3 action-btn"
                >
                  <v-icon left size="24">mdi-help-plus</v-icon>
                  Crear Pregunta
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="success"
                  variant="elevated"
                  block
                  size="x-large"
                  @click="refreshData"
                  class="mb-3 action-btn"
                  :loading="loading"
                >
                  <v-icon left size="24">mdi-refresh</v-icon>
                  Actualizar Datos
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="warning"
                  variant="elevated"
                  block
                  size="x-large"
                  @click="exportData"
                  class="mb-3 action-btn"
                >
                  <v-icon left size="24">mdi-download</v-icon>
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
  { title: 'Usuario', key: 'username', sortable: true },
  { title: 'Email', key: 'email', sortable: true },
  { title: 'Estado', key: 'isActive', sortable: true },
  { title: 'Fecha Creación', key: 'createdAt', sortable: true }
]

const questionHeaders = [
  { title: 'ID', key: 'questionId', sortable: true },
  { title: 'Pregunta', key: 'question', sortable: true },
  { title: 'Estado', key: 'isActive', sortable: true }
]

const formatDate = (dateString) => {
  return new Date(dateString).toLocaleDateString('es-ES', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  })
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

<style scoped>
.dashboard-container {
  padding: 20px;
  max-width: 1400px;
  margin: 0 auto;
}

.stats-card {
  border-radius: 16px;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  height: 100%;
  min-height: 200px;
}

.stats-card:hover {
  transform: translateY(-4px);
  box-shadow: 0 12px 30px rgba(0, 0, 0, 0.15) !important;
}

.v-card {
  border-radius: 16px;
}

.action-btn {
  border-radius: 12px;
  transition: all 0.3s ease;
  height: 60px;
}

.action-btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.15) !important;
}

.h-100 {
  height: 100%;
}

/* Mejoras para pantallas grandes */
@media (min-width: 1200px) {
  .dashboard-container {
    padding: 40px;
  }
  
  .stats-card {
    min-height: 220px;
  }
}

/* Responsive adjustments */
@media (max-width: 600px) {
  .text-h3 {
    font-size: 2rem !important;
  }
  
  .stats-card {
    margin-bottom: 16px;
  }
  
  .dashboard-container {
    padding: 16px;
  }
}
</style> 