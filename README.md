<img width="4032" height="913" alt="image" src="https://github.com/user-attachments/assets/3a53997d-cd95-44f1-a897-7154a4054c0f" /><img width="4032" height="913" alt="image" src="https://github.com/user-attachments/assets/3a53997d-cd95-44f1-a897-7154a4054c0f" /><img width="4032" height="913" alt="image" src="https://github.com/user-attachments/assets/3a53997d-cd95-44f1-a897-7154a4054c0f" />Sistema de Asistencias Vehiculares
Módulo “Asignación Inteligente de Proveedores”

Descripción
Plataforma compuesta por un microservicio .NET para la asignación óptima de proveedores y un frontend React (mock) para login y consulta de estado/ETA. El objetivo es ofrecer un “esqueleto productivo” con buenas prácticas de arquitectura, calidad, seguridad y despliegue.

2. Estructura del repositorio
.
├─ ProviderOptimizerService.API/                 # ASP.NET Core Web API
├─ ProviderOptimizerService.Application/         # Casos de uso, puertos
├─ ProviderOptimizerService.Domain/              # Entidades / VO / reglas
├─ ProviderOptimizerService.Infrastructure/      # EF Core, DbContext, Outbox, etc.
├─ ProviderOptimizerService.UnitTests/           # Pruebas de unidad
├─ ProviderOptimizerService.IntegrationTests/    # Pruebas de integración (HTTP)
├─ frontend/                                     # React (mock) - zip entregado
│  └─ react_simple_fe_auth/                      # Proyecto Vite listo para ejecutar
└─ .github/workflows/
   ├─ ci.yml                                     # Build · Lint · Tests · Docker build
   └─ release.yml                                # (opcional) Push ECR

2. Frontend (React mock)

Vite + React 18, React Router 6.
Context global de autenticación (AuthContext) con token simulado en localStorage.
Login con admin/admin, guard de ruta /providers.
Página “Proveedores” con tabla (estado, rating, costo/km, ETA simulado):

ETA se calcula por Haversine + velocidad base (35 km/h) + jitter ±15%.


Estilos en src/styles/app.css (tema oscuro).
Encabezado:
“Sistema de Asistencias Vehiculares – Módulo ‘Asignación Inteligente de Proveedores’”.

Cómo levantar (carpeta: react_simple_fe_auth/):
Shellnpm installnpm run dev# http://localhost:5173# Credenciales demo: admin / admin

 Runbook local
 API en modo dev
 # desde la raíz de la solución
dotnet restore
dotnet build
dotnet run --project ProviderOptimizerService.API/ProviderOptimizerService.API.csproj
# Swagger: http://localhost:8080/swagger

docker build -t provider-optimizer-api -f ProviderOptimizerService.API/Dockerfile .
docker run -p 8080:8080 provider-optimizer-api


Base de datos (si la usas local):

PostgreSQL en Docker (postgres:16) expuesto en 5432.
Conexión típica: Host=127.0.0.1;Port=5432;Database=optimizer;Username=optimizer;Password=optimizer.
  
      
