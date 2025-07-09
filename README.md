# Microservicio Xia - Sistema de Usuarios con Recuperación de Contraseñas

## Descripción

Microservicio en .NET 9.0.105 que implementa un CRUD completo de usuarios con sistema de recuperación de contraseñas mediante preguntas de seguridad. Utiliza Clean Architecture y MongoDB Atlas.

## Características

- ✅ CRUD completo de usuarios
- ✅ Sistema de preguntas de seguridad
- ✅ Recuperación de contraseñas
- ✅ Clean Architecture
- ✅ MongoDB Atlas
- ✅ API REST con Swagger

## Instalación

```bash
dotnet restore
dotnet run --project MicroservicioXia.API

cd MicroservicioXia.Admin
nom install
npm run dev
```

## Endpoints Principales

### Usuarios
- `GET /api/users` - Obtener todos los usuarios
- `GET /api/users/{id}` - Obtener usuario por ID
- `POST /api/users` - Crear usuario
- `PUT /api/users/{id}` - Actualizar usuario
- `DELETE /api/users/{id}` - Eliminar usuario

### Recuperación de Contraseñas
- `GET /api/users/password-recovery/{email}` - Obtener preguntas de seguridad
- `POST /api/users/password-reset` - Restablecer contraseña

### Preguntas de Seguridad
- `GET /api/securityquestions` - Obtener plantillas
- `GET /api/securityquestions/active` - Obtener plantillas activas
- `POST /api/securityquestions` - Crear plantilla

## Documentación

Accede a Swagger en: `https://localhost:7001/swagger` 

Accede al FronEnd en `http://localhost:5173`
