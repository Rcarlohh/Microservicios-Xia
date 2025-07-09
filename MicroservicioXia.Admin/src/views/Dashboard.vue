<template>
  <div>
    <v-row>
      <v-col cols="12">
        <div class="d-flex align-center mb-6">
          <v-icon size="32" color="primary" class="mr-3">mdi-view-dashboard</v-icon>
          <h1 class="text-h4 font-weight-light">Dashboard de Administración</h1>
        </div>
      </v-col>
    </v-row>

    <!-- Tarjetas de estadísticas -->
    <v-row>
      <v-col cols="12" sm="6" md="3">
        <v-card class="mx-auto stats-card" elevation="2">
          <v-card-text class="text-center pa-6">
            <v-icon size="48" color="primary" class="mb-3">mdi-account-group</v-icon>
            <div class="text-h3 font-weight-bold text-primary mb-2">
              {{ stats.totalUsers }}
            </div>
            <div class="text-subtitle-1 text-medium-emphasis">Total de Usuarios</div>
          </v-card-text>
          <v-card-actions class="pa-4 pt-0">
            <v-btn
              color="primary"
              variant="text"
              block
              @click="$router.push('/users')"
            >
              Ver Usuarios
              <v-icon right>mdi-arrow-right</v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card class="mx-auto stats-card" elevation="2">
          <v-card-text class="text-center pa-6">
            <v-icon size="48" color="success" class="mb-3">mdi-account-check</v-icon>
            <div class="text-h3 font-weight-bold text-success mb-2">
              {{ stats.activeUsers }}
            </div>
            <div class="text-subtitle-1 text-medium-emphasis">Usuarios Activos</div>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card class="mx-auto stats-card" elevation="2">
          <v-card-text class="text-center pa-6">
            <v-icon size="48" color="info" class="mb-3">mdi-help-circle</v-icon>
            <div class="text-h3 font-weight-bold text-info mb-2">
              {{ stats.totalQuestions }}
            </div>
            <div class="text-subtitle-1 text-medium-emphasis">Preguntas de Seguridad</div>
          </v-card-text>
          <v-card-actions class="pa-4 pt-0">
            <v-btn
              color="info"
              variant="text"
              block
              @click="$router.push('/security-questions')"
            >
              Ver Preguntas
              <v-icon right>mdi-arrow-right</v-icon>
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols="12" sm="6" md="3">
        <v-card class="mx-auto stats-card" elevation="2">
          <v-card-text class="text-center pa-6">
            <v-icon size="48" color="warning" class="mb-3">mdi-help-circle-outline</v-icon>
            <div class="text-h3 font-weight-bold text-warning mb-2">
              {{ stats.activeQuestions }}
            </div>
            <div class="text-subtitle-1 text-medium-emphasis">Preguntas Activas</div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Gráficos y tablas -->
    <v-row class="mt-8">
      <v-col cols="12" lg="6">
        <v-card elevation="2">
          <v-card-title class="d-flex align-center">
            <v-icon left color="primary">mdi-account-group</v-icon>
            <span class="text-h6">Usuarios Recientes</span>
          </v-card-title>
          <v-card-text class="pa-0">
            <v-data-table
              :headers="userHeaders"
              :items="recentUsers"
              :loading="loading"
              :items-per-page="5"
              class="elevation-0"
              density="compact"
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

      <v-col cols="12" lg="6">
        <v-card elevation="2">
          <v-card-title class="d-flex align-center">
            <v-icon left color="info">mdi-help-circle</v-icon>
            <span class="text-h6">Preguntas de Seguridad</span>
          </v-card-title>
          <v-card-text class="pa-0">
            <v-data-table
              :headers="questionHeaders"
              :items="securityQuestions"
              :loading="loading"
              :items-per-page="5"
              class="elevation-0"
              density="compact"
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
    <v-row class="mt-8">
      <v-col cols="12">
        <v-card elevation="2">
          <v-card-title class="d-flex align-center">
            <v-icon left color="warning">mdi-lightning-bolt</v-icon>
            <span class="text-h6">Acciones Rápidas</span>
          </v-card-title>
          <v-card-text>
            <v-row>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="primary"
                  variant="elevated"
                  block
                  size="large"
                  @click="$router.push('/users')"
                  class="mb-2"
                >
                  <v-icon left>mdi-account-plus</v-icon>
                  Crear Usuario
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="info"
                  variant="elevated"
                  block
                  size="large"
                  @click="$router.push('/security-questions')"
                  class="mb-2"
                >
                  <v-icon left>mdi-help-plus</v-icon>
                  Crear Pregunta
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="success"
                  variant="elevated"
                  block
                  size="large"
                  @click="refreshData"
                  class="mb-2"
                  :loading="loading"
                >
                  <v-icon left>mdi-refresh</v-icon>
                  Actualizar Datos
                </v-btn>
              </v-col>
              <v-col cols="12" sm="6" md="3">
                <v-btn
                  color="warning"
                  variant="elevated"
                  block
                  size="large"
                  @click="exportData"
                  class="mb-2"
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
.stats-card {
  border-radius: 12px;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.stats-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15) !important;
}

.v-card {
  border-radius: 12px;
}

/* Responsive adjustments */
@media (max-width: 600px) {
  .text-h3 {
    font-size: 2rem !important;
  }
  
  .stats-card {
    margin-bottom: 16px;
  }
}
</style> 