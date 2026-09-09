# 🔨 SubastaYa - Plataforma de Subastas en Tiempo Real

Proyecto integral de comercio electrónico y subastas en tiempo real con respaldo transaccional atómico (**Escrow**) y protección contra ofertas tardías (**Anti-Sniping**), desarrollado para la cátedra **Proyecto de Software** de la **Universidad Nacional Arturo Jauretche (UNAJ)**.

---

## 🏛️ Arquitectura del Sistema (Clean Architecture)

El backend sigue los lineamientos estrictos de **Clean Architecture (Arquitectura Limpia)** desacoplado en 4 proyectos:

```text
SubastaYa-Backend/
├── SubastaYa.Domain/          # Núcleo puro: Entidades, Enums, Excepciones y Contratos de Repositorios.
├── SubastaYa.Application/     # Casos de uso: DTOs, Servicios de Negocio (Escrow, Anti-Sniping, Billetera).
├── SubastaYa.Infrastructure/  # Acceso a datos: DbContext, Fluent API, Migraciones EF Core y Repositorios.
├── SubastaYa.API/             # Presentación: Controladores RESTful, Middleware de Excepciones y Background Worker.
└── SubastaYa.slnx             # Archivo de solución unificado.


⚙️ Stack Tecnológico
Lenguaje & Runtime: C# | .NET 8.0 LTS.
ORM: Entity Framework Core 8 (Enfoque Code-First con Migraciones).
Motor de Base de Datos: Microsoft SQL Server (LocalDB / Express).
Manejo de Concurrencia: Optimistic Locking con token de concurrencia (Version).
Transaccionalidad: Bloques atómicos ACID (TransactionScope) en operaciones financieras de Escrow.
Procesos en Segundo Plano: IHostedService (BackgroundService) con inyección de IServiceScopeFactory.
Documentación de API: OpenAPI / Swagger UI interactivo vía Swashbuckle.AspNetCore.

.
🚀 Guía de Instalación y Puesta en Marcha
Prerrequisitos
.NET 8.0 SDK instalado (dotnet --version >= 8.0.x).
Microsoft SQL Server LocalDB (incluido con Visual Studio o instalable con sqllocaldb).
Pasos para ejecutar:

1. Clonar el repositorio:

code Bash

git clone https://github.com/EmanuelEspinosa/SubastaYa-Backend.git
cd SubastaYa-Backend


2. Configurar la cadena de conexión (si aplica):
Por defecto, SubastaYa.API/appsettings.json apunta a SQL Server LocalDB:

code JSON

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SubastaYaDb;Trusted_Connection=True;TrustServerCertificate=True;"
}


3. Compilar la solución:

code Bash

dotnet build


4. Ejecutar la API:

code Bash

dotnet run --project SubastaYa.API

(Al iniciar, la API aplica automáticamente las migraciones pendientes y carga el Seed Data si la base de datos no existe).

5. Acceder a Swagger UI:

Abrir en el navegador: http://localhost:5120/swagger (o https://localhost:7000/swagger).



📌 Datos Semilla Iniciales (Seed Data)
El sistema precarga automáticamente los siguientes escenarios para pruebas:

. 4 Usuarios y Billeteras:
vendedor@test.com (Saldo Total: $0.00).
comprador1@test.com (Total: $150,000 | Retenido: $45,000 | Disponible: $105,000).
comprador2@test.com (Total: $200,000 | Disponible: $200,000).
sinfondos@test.com (Total: $500.00 | Disponible: $500.00).

. 4 Categorías: Tecnología, Coleccionables, Indumentaria, Vehículos.

. 5 Subastas (Casos de prueba):
1 - Activa estándar: Cierra en 30 min (lidera Comprador 1 con $45,000).
2 - Activa crítica: Cierra en < 2 min (lista para probar extensión Anti-Sniping).
3 - Próxima: Inicio programado a +24 hs (pujas bloqueadas).
4 - Vencida con ganador: Liquidada por el Worker automáticamente a estado Finalizada.
5 - Vencida desierta: Cerrada por el Worker automáticamente a estado Desierta.
. Libro Mayor (TransaccionesLedger): Asientos contables que respaldan los saldos y retenciones desde el inicio.


📡 Endpoints de la API REST
Subastas (/api/auctions)
Método	Endpoint	Descripción	Códigos HTTP
GET	/api/auctions	Listado con filtros por estado (?estado=) y categoría (?categoriaId=).	200 OK
GET	/api/auctions/{id}	Detalle completo de una subasta y puja líder.	200 OK, 404 Not Found
POST	/api/auctions	Publicación de nueva subasta.	201 Created, 422 Unprocessable
POST	/api/auctions/{id}/bids	Registro de oferta en vivo (ejecuta Escrow y Anti-sniping).	200 OK, 409 Conflict, 422 Unprocessable
GET	/api/auctions/{id}/bids	Historial de pujas con anonimización de postores.	200 OK, 404 Not Found


Billetera (/api/wallet)
Método	Endpoint	Descripción	Códigos HTTP
GET	/api/wallet/balance?usuarioId={id}	Consulta de saldos (Total, Retenido, Disponible).	200 OK, 404 Not Found
POST	/api/wallet/deposit	Acreditación simulada de fondos con asiento en Ledger.	200 OK, 400 Bad Request

⚡ Prueba de Concurrencia Optimista (Stress Test - HTTP 409 Conflict)
Para validar el requerimiento estricto de concurrencia optimista ante ofertas simultáneas en el mismo milisegundo:
Ejecución con script de Bash (Linux / macOS / Git Bash):

code Bash

#!/bin/bash
# Disparo en paralelo de dos ofertas idénticas con la misma versión inicial
curl -i -X POST http://localhost:5120/api/auctions/1/bids \
  -H "Content-Type: application/json" \
  -d '{"subastaId":1,"compradorId":3,"monto":50000}' &

curl -i -X POST http://localhost:5120/api/auctions/1/bids \
  -H "Content-Type: application/json" \
  -d '{"subastaId":1,"compradorId":2,"monto":50000}' &

wait


Comportamiento Esperado:
1. La primera petición en ser confirmada actualiza la subasta, incrementa subasta.Version y retorna HTTP 200 OK.
2. La segunda petición colisiona con el token de concurrencia (Version desactualizada), provocando que Entity Framework Core dispare DbUpdateConcurrencyException.
3. El ExceptionMiddleware captura la excepción y retorna inmediatamente HTTP 409 Conflict con el payload:

code JSON

{
  "statusCode": 409,
  "message": "El recurso fue modificado concurrentemente por otra transacción. Reintente.",
  "errorType": "DbUpdateConcurrencyException"
}