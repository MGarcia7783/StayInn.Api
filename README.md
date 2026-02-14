# 🏨 StayInn API

**StayInn** es una solución robusta de backend diseñada para la gestión integral de servicios hoteleros. Esta API RESTful permite la administración de habitaciones, áreas de esparcimiento, y un sistema transaccional de reservaciones, todo bajo una arquitectura escalable y mantenible.


## 🚀 Tecnologías y Herramientas

- Lenguaje: C# 10 / .NET 10  
- Framework: ASP.NET Core Web API 
- Arquitectura: Clean Architecture
- Persistencia: Entity Framework Core con PostgreSQL  
- Seguridad: Autenticación y Autorización basada en Roles con ASP.NET Core Identity y JWT (JSON Web Tokens) 
- Documentación: Swagger / OpenAPI 


## 🏗️ Estructura del Proyecto
El proyecto se divide en 4 capas principales siguiendo los principios de la arquitectura limpia:
**- StayInn.Domain:** Contiene las entidades principales, enums y las interfaces de los repositorios. Es el corazón del negocio y no tiene dependencias externas.
**- StayInn.Application:** Define la lógica de negocio, servicios, DTOs y perfiles de mapeo (AutoMapper).
**- StayInn.Infrastructure:** Implementa el acceso a datos (PostgreSQL), el ApplicationDbContext, las migraciones y servicios de infraestructura.
**- StayInn.Api:** Punto de entrada de la aplicación. Contiene los controladores, la configuración de la inyección de dependencias y los middlewares.


## 🛠️ Funcionalidades Principales
✅ Gestión de Habitaciones: CRUD completo y control de disponibilidad.
✅ Sistema de Reservas: Motor de reservaciones con cálculo automático de montos y manejo de estados (Pendiente, Confirmada, Cancelada).
✅ Áreas de Esparcimiento: Administración de zonas comunes del hotel para la experiencia del cliente.
✅ Geolocalización: Soporte para coordenadas (Latitud/Longitud) para integración con mapas en el frontend.


## 🚀 Empezando

Sigue estas instrucciones para obtener una copia del proyecto en tu máquina local para desarrollo y pruebas.


## 📋 Pre-requisitos

Asegúrate de tener instalado:

- .NET SDK 10  
- PostgreSQL  
- DBeaver (opcional)  
- Postman  


## 🛠️ Instalación

📥 Clona el repositorio:

```bash
git clone https://github.com/MGarcia7783/StayInn.Api.git
```

⚙ Configura las variables de entorno en el archivo .env

```env
DB_SERVER=localhost
DB_DATABASE=StayInnDB
DB_USER=TuUsuario
DB_PASSWORD=TuPassword
```

🗄️ Crear la base de datos y aplicar migraciones
```bash
Add-Migration "Título para tu migración"
Update-Database
```
