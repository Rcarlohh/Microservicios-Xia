# Panel de Administración - Microservicio Xia

Interfaz de administración para el microservicio Xia, desarrollada con Vue.js 3 y Vuetify.

## Características

- 🔐 **Autenticación de Administrador**: Login seguro con JWT
- 👥 **Gestión de Usuarios**: CRUD completo de usuarios del sistema
- ❓ **Gestión de Preguntas de Seguridad**: Administración de preguntas de seguridad
- 📊 **Dashboard**: Estadísticas y resumen del sistema
- 🎨 **Interfaz Moderna**: Diseño responsivo con Vuetify
- 🔒 **Rutas Protegidas**: Navegación segura con guardias de autenticación

## Requisitos Previos

- Node.js (versión 16 o superior)
- npm o yarn
- Backend del microservicio Xia ejecutándose en `https://localhost:7226`

## Instalación

1. **Clonar el repositorio** (si no lo has hecho ya):
```bash
git clone <url-del-repositorio>
cd Microservicios-Xia/MicroservicioXia.Admin
```

2. **Instalar dependencias**:
```bash
npm install
```

3. **Ejecutar en modo desarrollo**:
```bash
npm run dev
```

4. **Abrir en el navegador**:
```
http://localhost:5173
```

## Configuración

### Variables de Entorno

Crea un archivo `.env` en la raíz del proyecto:

```env
VITE_API_BASE_URL=https://localhost:7226/api
```

### Configuración del Backend

Asegúrate de que el backend esté configurado correctamente:

1. **CORS habilitado** para `http://localhost:5173`
2. **Endpoints de administrador** disponibles
3. **Base de datos MongoDB** conectada

## Uso

### 1. Iniciar Sesión

- Accede a `http://localhost:5173`
- Usa las credenciales de administrador
- El sistema redirigirá automáticamente al dashboard

### 2. Dashboard

- **Estadísticas**: Vista general de usuarios y preguntas
- **Acciones Rápidas**: Enlaces directos a funciones principales
- **Datos Recientes**: Últimos usuarios y preguntas creadas

### 3. Gestión de Usuarios

- **Listar**: Ver todos los usuarios del sistema
- **Crear**: Agregar nuevos usuarios
- **Editar**: Modificar información de usuarios existentes
- **Eliminar**: Remover usuarios del sistema
- **Filtros**: Buscar por nombre, email, rol o estado

### 4. Gestión de Preguntas de Seguridad

- **Listar**: Ver todas las preguntas disponibles
- **Crear**: Agregar nuevas preguntas de seguridad
- **Editar**: Modificar preguntas existentes
- **Eliminar**: Remover preguntas del sistema
- **Filtros**: Buscar por texto o estado

## Estructura del Proyecto

```
src/
├── views/
│   ├── Login.vue              # Pantalla de login
│   ├── Dashboard.vue          # Dashboard principal
│   ├── Users.vue             # Gestión de usuarios
│   └── SecurityQuestions.vue # Gestión de preguntas
├── components/               # Componentes reutilizables
├── assets/                  # Recursos estáticos
├── App.vue                  # Componente raíz
└── main.js                  # Punto de entrada
```

## API Endpoints Utilizados

### Autenticación
- `POST /api/users/admin/login` - Login de administrador

### Usuarios
- `GET /api/users` - Listar usuarios
- `POST /api/users` - Crear usuario
- `PUT /api/users/{id}` - Actualizar usuario
- `DELETE /api/users/{id}` - Eliminar usuario
- `PUT /api/users/{id}/role` - Cambiar rol de usuario

### Preguntas de Seguridad
- `GET /api/securityquestions` - Listar preguntas
- `POST /api/securityquestions` - Crear pregunta
- `PUT /api/securityquestions/{id}` - Actualizar pregunta
- `DELETE /api/securityquestions/{id}` - Eliminar pregunta

## Tecnologías Utilizadas

- **Vue.js 3**: Framework de JavaScript
- **Vuetify 3**: Framework de UI
- **Vue Router 4**: Enrutamiento
- **Axios**: Cliente HTTP
- **Material Design Icons**: Iconografía

## Desarrollo

### Comandos Disponibles

```bash
# Desarrollo
npm run dev

# Construcción para producción
npm run build

# Vista previa de producción
npm run preview

# Linting
npm run lint
```

### Agregar Nuevas Funcionalidades

1. **Nueva Vista**: Crear archivo en `src/views/`
2. **Nueva Ruta**: Agregar en `src/main.js`
3. **Nuevo Componente**: Crear en `src/components/`

## Seguridad

- **JWT Tokens**: Autenticación basada en tokens
- **Interceptores**: Manejo automático de errores 401
- **Guardias de Ruta**: Protección de rutas privadas
- **Validación**: Validación de formularios en frontend

## Soporte

Para problemas o preguntas:

1. Revisa la documentación del backend
2. Verifica la configuración de CORS
3. Comprueba la conectividad con la API
4. Revisa los logs del navegador

## Licencia

Este proyecto es parte del microservicio Xia.
