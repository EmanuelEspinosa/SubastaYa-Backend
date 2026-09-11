# 🔨 SubastaYa - Plataforma de Subastas en Tiempo Real

Proyecto integral de comercio electrónico y subastas en tiempo real con respaldo transaccional atómico (**Escrow**), extensión dinámica contra ofertas de último segundo (**Anti-Sniping**) y control de concurrencia optimista, desarrollado para la cátedra **Proyecto de Software** de la **Universidad Nacional Arturo Jauretche (UNAJ)**.

> 🌐 **Interfaz de Usuario (Frontend):**  
> El cliente web de la aplicación se encuentra disponible en: **[Enlace al Repositorio del Frontend]** *(o en el directorio `/frontend` de esta solución)*.

---

## 🏛️ Arquitectura del Sistema (Clean Architecture)

El backend implementa de forma estricta los principios de **Clean Architecture** (Arquitectura Limpia), separando responsabilidades en cuatro capas desacopladas:

```
SubastaYa-Backend/
├── SubastaYa.Domain/          # Núcleo puro: Entidades, Enums, Excepciones y Contratos de Repositorios.
├── SubastaYa.Application/     # Casos de uso: DTOs, Servicios de Dominio (Escrow, Anti-Sniping, Billetera).
├── SubastaYa.Infrastructure/  # Acceso a datos: DbContext, Migraciones Code-First y Repositorios EF Core.
├── SubastaYa.API/             # Presentación: Controladores RESTful, Middleware de Excepciones y Worker.
└── SubastaYa.slnx             # Archivo de solución unificado.
```

---

## ⚙️ Stack Tecnológico

* **Lenguaje & Runtime:** C# | .NET 8.0 LTS.
* **ORM:** Entity Framework Core 8 (Enfoque Code-First con Migraciones).
* **Motor de Base de Datos:** Microsoft SQL Server (LocalDB / Express).
* **Manejo de Concurrencia:** Optimistic Locking con token de concurrencia (`Version`).
* **Transaccionalidad Financiera (ACID):** Bloques atómicos con `TransactionScope` para las operaciones de Escrow.
* **Procesos en Segundo Plano:** `BackgroundService` (`AuctionClosingWorker`) para el cierre y liquidación periódica.
* **Documentación de API:** OpenAPI / Swagger UI interactivo vía Swashbuckle.

---

## ⏱️ Sincronización en Tiempo Real (Sala de Subastas)

Siguiendo las alternativas contempladas en la **página 5 del enunciado de la cátedra**, la sincronización de la sala en vivo (reloj visual regresivo, alerta de zona crítica, extensión Anti-Sniping y detección de superación/outbid) se implementa mediante la técnica de **Short-Polling asíncrono**:

* El cliente web realiza peticiones asíncronas cada **2 a 3 segundos** a los endpoints:
  * `GET /api/auctions/{id}`: Sincroniza el temporizador regresivo y detecta si la `fechaFin` fue extendida por Anti-Sniping.
  * `GET /api/auctions/{id}/bids`: Actualiza el historial cronológico de ofertas y notifica al usuario si su oferta fue superada en tiempo real.
* Esta estrategia desacopla el transporte, evita saturación de sockets persistentes y mantiene una respuesta de sub-milisegundos gracias a los índices de base de datos.

---

## 🚀 Guía de Instalación y Puesta en Marcha

### Prerrequisitos
* **.NET 8.0 SDK** instalado (`dotnet --version >= 8.0.x`).
* **Microsoft SQL Server LocalDB** (incluido con Visual Studio o instalable mediante `sqllocaldb`).

### Pasos para ejecutar:

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/EmanuelEspinosa/SubastaYa-Backend.git
   cd SubastaYa-Backend
   ```

2. **Configuración de Conexión:**  
   Por defecto, `SubastaYa.API/appsettings.json` apunta a SQL Server LocalDB:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SubastaYaDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Compilar la solución:**
   ```bash
   dotnet build
   ```

4. **Ejecutar la API:**
   ```bash
   dotnet run --project SubastaYa.API
   ```
   *(Al arrancar, la API ejecuta automáticamente `Database.Migrate()` aplicando las migraciones pendientes y cargando el Seed Data inicial).*

5. **Acceder a la Documentación Interactiva:**  
   Abrir en el navegador: [http://localhost:5120/swagger](http://localhost:5120/swagger) (o `https://localhost:7000/swagger`).

---

## 📌 Datos Semilla Obligatorios (Seed Data)

El sistema inicializa la base de datos con los escenarios exactos pedidos por la cátedra para la mesa de evaluación:

* **4 Usuarios y Billeteras:**
  * `vendedor@test.com`: Saldo Total: $0.00.
  * `comprador1@test.com`: Total: $150,000 | Retenido: $45,000 | Disponible: $105,000 (Postor líder subasta 1).
  * `comprador2@test.com`: Total: $200,000 | Disponible: $200,000 (Postor habilitado solvente).
  * `sinfondos@test.com`: Total: $500.00 | Disponible: $500.00 (Caso de prueba para rechazo por saldo insuficiente).
* **4 Categorías:** Tecnología, Coleccionables, Indumentaria, Vehículos.
* **5 Subastas (Casos de prueba):**
  1. **Activa estándar:** Cierra en 30 min (con 2 pujas previas cargadas; lidera Comprador 1 con $45,000).
  2. **Activa crítica:** Cierra en < 2 min (lista para probar alerta visual y extensión Anti-Sniping).
  3. **Próxima:** Inicio programado a +24 hs (pujas bloqueadas por regla de negocio).
  4. **Vencida con ganador:** Liquidada por el Worker automáticamente a estado `Finalizada` con transferencia de fondos.
  5. **Vencida desierta:** Pasada por el Worker automáticamente a estado `Desierta`.
* **Libro Mayor (`TransaccionesLedger`):** Asientos contables que justifican desde el inicio los depósitos y el saldo retenido de $45,000.

---

## 📡 Endpoints de la API REST

### Subastas (`/api/auctions`)
| Método | Endpoint | Descripción | Códigos de Estado HTTP |
|:---:|---|---|---|
| **GET** | `/api/auctions` | Catálogo con filtros por estado (`?estado=`) y categoría (`?categoriaId=`). | `200 OK` |
| **GET** | `/api/auctions/{id}` | Detalle completo de la subasta, estado y puja líder. | `200 OK`, `404 Not Found` |
| **POST** | `/api/auctions` | Creación de nueva subasta con validación de fechas y precios. | `201 Created`, `422 Unprocessable` |
| **POST** | `/api/auctions/{id}/bids` | Registro de puja (evalúa saldo, ejecuta Escrow atómico y Anti-Sniping). | `200 OK`, `409 Conflict`, `422 Unprocessable` |
| **GET** | `/api/auctions/{id}/bids` | Historial cronológico de pujas anonimizadas para la sala en vivo. | `200 OK`, `404 Not Found` |

### Billetera Virtual (`/api/wallet`)
| Método | Endpoint | Descripción | Códigos de Estado HTTP |
|:---:|---|---|---|
| **GET** | `/api/wallet/balance?usuarioId={id}` | Desglose de saldos: Total, Retenido en Escrow y Disponible. | `200 OK`, `404 Not Found` |
| **POST** | `/api/wallet/deposit` | Acreditación simulada de fondos con asiento en Ledger y auditoría. | `200 OK`, `400 Bad Request` |

---

## ⚡ Prueba de Concurrencia Optimista (Stress Test - HTTP 409 Conflict)

En cumplimiento con la **Sección 4.1 del documento de la cátedra**, se implementó el mecanismo de **Optimistic Locking** para evitar que dos postores puedan registrar ofertas líderes en el mismo milisegundo basándose en un estado desactualizado.

### Script de Prueba de Estrés (Bash / Linux / macOS / Git Bash):

```bash
#!/bin/bash
# Disparo en paralelo de dos ofertas simultáneas para la misma subasta con la misma versión inicial

echo ">> Disparando pujas concurrentes al mismo milisegundo..."

curl -i -X POST http://localhost:5120/api/auctions/1/bids \
  -H "Content-Type: application/json" \
  -d '{"subastaId":1,"compradorId":3,"monto":50000}' &

curl -i -X POST http://localhost:5120/api/auctions/1/bids \
  -H "Content-Type: application/json" \
  -d '{"subastaId":1,"compradorId":2,"monto":50000}' &

wait
echo ">> Fin de la prueba."
```

### Justificación Técnica del Resultado:
1. **Primera Petición procesada:** Encuentra la subasta en su versión actual, procesa el Escrow de forma atómica dentro de un `TransactionScope`, incrementa `subasta.Version++` y responde **HTTP 200 OK**.
2. **Segunda Petición procesada:** Intenta persistir cambios con la versión desactualizada. SQL Server detecta la discrepancia en el `WHERE Id = @id AND Version = @versionOriginal`, afectando 0 filas y provocando que Entity Framework Core dispare una `DbUpdateConcurrencyException`.
3. **Manejo Semántico de Excepciones:** Nuestro `ExceptionMiddleware` intercepta la excepción y retorna de inmediato **HTTP 409 Conflict** con el siguiente payload:

```json
{
  "statusCode": 409,
  "message": "El recurso fue modificado concurrentemente por otra transacción. Reintente.",
  "errorType": "DbUpdateConcurrencyException"
}
```
