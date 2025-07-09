<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const drawer = ref(true)

const isAuthenticated = computed(() => {
  return !!localStorage.getItem('adminToken')
})

const adminUser = computed(() => {
  const userStr = localStorage.getItem('adminUser')
  return userStr ? JSON.parse(userStr) : null
})

const logout = () => {
  localStorage.removeItem('adminToken')
  localStorage.removeItem('adminUser')
  router.push('/login')
}

onMounted(() => {
  // Si no está autenticado y no está en login, redirigir
  if (!isAuthenticated.value && router.currentRoute.value.path !== '/login') {
    router.push('/login')
  }
})
</script>

<template>
  <v-app>
    <!-- Barra de navegación superior -->
    <v-app-bar
      v-if="isAuthenticated"
      color="primary"
      dark
      app
      elevation="2"
    >
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      
      <v-toolbar-title class="text-h6 font-weight-medium">
        <v-icon left>mdi-shield-account</v-icon>
        Panel de Administración - Xia
      </v-toolbar-title>
      
      <v-spacer></v-spacer>
      
      <v-menu offset-y>
        <template v-slot:activator="{ props }">
          <v-btn
            icon
            v-bind="props"
            class="mr-2"
          >
            <v-icon>mdi-account-circle</v-icon>
          </v-btn>
        </template>
        <v-list>
          <v-list-item @click="logout">
            <v-list-item-title>
              <v-icon left>mdi-logout</v-icon>
              Cerrar Sesión
            </v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>

    <!-- Navegación lateral -->
    <v-navigation-drawer
      v-if="isAuthenticated"
      v-model="drawer"
      app
      color="grey-lighten-4"
      width="300"
      class="admin-drawer"
    >
      <v-list class="pa-6">
        <v-list-item
          prepend-avatar="https://cdn.vuetifyjs.com/images/john.jpg"
          :title="adminUser?.username || 'Administrador'"
          subtitle="Administrador del Sistema"
          class="mb-6 admin-user-info"
        ></v-list-item>
      </v-list>

      <v-divider></v-divider>

      <v-list density="compact" nav class="pa-4">
        <v-list-item
          prepend-icon="mdi-view-dashboard"
          title="Dashboard"
          value="dashboard"
          @click="$router.push('/dashboard')"
          :active="$route.path === '/dashboard'"
          class="mb-2 nav-item"
          rounded="lg"
        ></v-list-item>
        
        <v-list-item
          prepend-icon="mdi-account-group"
          title="Usuarios"
          value="users"
          @click="$router.push('/users')"
          :active="$route.path === '/users'"
          class="mb-2 nav-item"
          rounded="lg"
        ></v-list-item>
        
        <v-list-item
          prepend-icon="mdi-help-circle"
          title="Preguntas de Seguridad"
          value="security-questions"
          @click="$router.push('/security-questions')"
          :active="$route.path === '/security-questions'"
          class="mb-2 nav-item"
          rounded="lg"
        ></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- Contenido principal -->
    <v-main class="admin-main">
      <v-container fluid class="pa-8">
        <router-view></router-view>
      </v-container>
    </v-main>
  </v-app>
</template>

<style>
.v-main {
  background-color: #f8f9fa;
  min-height: 100vh;
}

.admin-main {
  background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
}

.admin-drawer {
  border-right: 1px solid rgba(0, 0, 0, 0.12);
  box-shadow: 2px 0 10px rgba(0, 0, 0, 0.1);
}

.admin-user-info {
  background-color: rgba(var(--v-theme-primary), 0.05);
  border-radius: 12px;
  margin-bottom: 24px !important;
}

.nav-item {
  margin-bottom: 8px;
  transition: all 0.3s ease;
}

.nav-item:hover {
  background-color: rgba(var(--v-theme-primary), 0.08);
  transform: translateX(4px);
}

.v-list-item--active {
  background-color: rgba(var(--v-theme-primary), 0.15) !important;
  color: rgb(var(--v-theme-primary)) !important;
  box-shadow: 0 2px 8px rgba(var(--v-theme-primary), 0.2);
}

.v-list-item--active .v-list-item__prepend {
  color: rgb(var(--v-theme-primary)) !important;
}

/* Mejoras para pantallas grandes */
@media (min-width: 1200px) {
  .admin-drawer {
    width: 320px !important;
  }
  
  .v-container {
    padding: 32px !important;
  }
}

/* Responsive adjustments */
@media (max-width: 960px) {
  .v-navigation-drawer {
    width: 280px !important;
  }
}

@media (max-width: 600px) {
  .v-container {
    padding: 16px !important;
  }
  
  .v-toolbar-title {
    font-size: 1.1rem !important;
  }
  
  .admin-drawer {
    width: 260px !important;
  }
}

/* Smooth transitions */
.v-list-item {
  transition: all 0.3s ease;
}

.v-list-item:hover {
  background-color: rgba(var(--v-theme-primary), 0.05);
}

/* Custom scrollbar */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb {
  background: #c1c1c1;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #a8a8a8;
}
</style>
