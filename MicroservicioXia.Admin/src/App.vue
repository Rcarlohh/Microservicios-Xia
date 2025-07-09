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
    >
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      
      <v-toolbar-title>Panel de Administración - Xia</v-toolbar-title>
      
      <v-spacer></v-spacer>
      
      <v-menu offset-y>
        <template v-slot:activator="{ props }">
          <v-btn
            icon
            v-bind="props"
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
    >
      <v-list>
        <v-list-item
          prepend-avatar="https://cdn.vuetifyjs.com/images/john.jpg"
          :title="adminUser?.username || 'Administrador'"
          subtitle="Administrador del Sistema"
        ></v-list-item>
      </v-list>

      <v-divider></v-divider>

      <v-list density="compact" nav>
        <v-list-item
          prepend-icon="mdi-view-dashboard"
          title="Dashboard"
          value="dashboard"
          @click="$router.push('/dashboard')"
          :active="$route.path === '/dashboard'"
        ></v-list-item>
        
        <v-list-item
          prepend-icon="mdi-account-group"
          title="Usuarios"
          value="users"
          @click="$router.push('/users')"
          :active="$route.path === '/users'"
        ></v-list-item>
        
        <v-list-item
          prepend-icon="mdi-help-circle"
          title="Preguntas de Seguridad"
          value="security-questions"
          @click="$router.push('/security-questions')"
          :active="$route.path === '/security-questions'"
        ></v-list-item>
      </v-list>
    </v-navigation-drawer>

    <!-- Contenido principal -->
    <v-main>
      <v-container fluid>
        <router-view></router-view>
      </v-container>
    </v-main>
  </v-app>
</template>

<style>
.v-main {
  background-color: #f5f5f5;
}
</style>
