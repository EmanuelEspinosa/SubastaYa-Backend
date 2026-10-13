# 🔨 SubastaYa - Plataforma de Subastas en Tiempo Real

Proyecto integral de comercio electrónico y subastas en tiempo real con respaldo transaccional atómico (**Escrow**), extensión dinámica contra ofertas de último segundo (**Anti-Sniping**) y control de concurrencia optimista, desarrollado para la cátedra **Proyecto de Software** de la **Universidad Nacional Arturo Jauretche (UNAJ)**.

> 🌐 **Interfaz de Usuario (Frontend):**  
> El cliente web de la aplicación se encuentra disponible en: **https://github.com/EmanuelEspinosa/SubastaYa-Frontend** 

---

## 🏛️ Arquitectura del Sistema (Clean Architecture + Custom CQRS)

El backend implementa de forma estricta los principios de **Clean Architecture** desglosando los casos de uso mediante el patrón **CQRS Custom (Command Query Responsibility Segregation)** con DTOs, `Commands`, `Queries` y `Handlers` dedicados por cada operación de lectura y escritura:

```
SubastaYa-Backend/
├── SubastaYa.Domain/          # Núcleo puro: Entidades, Enums, Excepciones y Contratos de Repositorios.
├── SubastaYa.Application/     # CQRS: Commands, Queries, Handlers, DTOs y Servicios (Escrow, Anti-Sniping).
├── SubastaYa.Infrastructure/  # Acceso a datos: DbContext, Migraciones Code-First, DbInitializer y Repositorios EF Core.
├── SubastaYa.API/             # Presentación: Controladores RESTful (Invocación a Handlers), Middleware y Worker.
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

El sistema inicializa la base de datos en tiempo de ejecución (`DbInitializer.cs`) calculando fechas dinámicas relativas al instante real de ejecución (`DateTime.UtcNow`), aplicando `RESEED` de identidades y garantizando la vigencia de los datos para la mesa de evaluación:

* **4 Usuarios y Billeteras:**
  * `vendedor@test.com`: Saldo Total: $0.00.
  * `comprador1@test.com`: Total: $150,000 | Retenido: $45,000 | Disponible: $105,000 (Postor líder subasta 1).
  * `comprador2@test.com`: Total: $200,000 | Disponible: $200,000 (Postor habilitado solvente).
  * `sinfondos@test.com`: Total: $500.00 | Disponible: $500.00 (Caso de prueba para rechazo por saldo insuficiente).
* **4 Categorías:** Tecnología, Coleccionables, Indumentaria, Vehículos.
* **5 Subastas (Casos de prueba relativas a `DateTime.UtcNow`):**
  1. **PlayStation 5 (Caso A - Activa estándar):** Cierra en 7 días (`AddDays(7)`). Posee 2 pujas registradas; lidera Comprador 1 con $45,000.
  2. **Apple iPhone 13 (Caso B - Activa extendida):** Cierra en 15 días (`AddDays(15)`).
  3. **Figura De Acción Articulada (Caso C - Próxima):** Inicia en 2 días (`AddDays(2)`) y finaliza en 10 días (pujas bloqueadas por regla de negocio).
  4. **Camiseta de Fútbol (Caso D - Vencida con ganador):** Venció ayer (`AddDays(-1)`), con oferta cargada de $35,000 de Comprador 1 (lista para liquidación por el Worker).
  5. **Bicicleta Kanji R/29 (Caso E - Vencida desierta):** Venció hace 2 días (`AddDays(-2)`) sin ofertas (lista para pasar a Desierta por el Worker).
* **Libro Mayor (`TransaccionesLedger`):** Asientos contables que respaldan depósitos, liberaciones y la retención inicial de $45,000.

---
## 🔒 Módulo de Autenticación & Seguridad (JWT + BCrypt)

La seguridad del sistema está diseñada protegiendo las operaciones financieras y de subasta mediante tokens criptográficos y cifrado de contraseñas.

### 1. Desacoplamiento con DTOs (Data Transfer Objects)
Para proteger el modelo del dominio y no exponer datos sensibles (como `PasswordHash`), la comunicación se realiza mediante contratos estrictos en `SubastaYa.Application.DTOs.Auth`:

* **`LoginDto`**: Captura `Email` y `Password` en texto plano.
* **`RegistroDto`**: Captura `Nombre`, `Email` y `Password`.
* **`AuthResponseDto`**: Devuelve al cliente el `UsuarioId`, `Nombre`, `Email` y el `Token` JWT.

### 2. Cifrado de Contraseñas con BCrypt
* **Protección de Credenciales:** Las contraseñas en texto plano enviadas desde el frontend son procesadas con `BCrypt.Net` generando un *Salting* automático e indivisible.
* **Verificación Unidireccional:** Durante el login (`AuthService.LoginAsync`), la clave ingresada se verifica contra el hash mediante `BCrypt.Verify()` sin desencriptar el valor almacenado.

### 3. Emisión de Tokens JWT (JSON Web Tokens)
Al autenticarse o registrarse exitosamente, la API genera un token de acceso firmado digitalmente:

* **Claims Integrados:** Contiene afirmaciones con la identidad del usuario (`NameIdentifier`, `Name`, `Email`), permitiendo a los controladores conocer quién realiza cada acción sin consultar la base de datos en cada petición.
* **Tiempo de Vida:** Configurado a **7 días** (`DateTime.UtcNow.AddDays(7)`). Cumplido ese plazo, el token expira exigiendo reautenticación.
* **Firma Criptográfica:** Firmado con la clave secreta simétrica mediante el algoritmo `HmacSha256Signature`.
* **Configuración CERO Tolerancia (`ClockSkew = TimeSpan.Zero`):** El token caduca exactamente al segundo programado.

### 4. Credencial Universal de Prueba  

Todos los usuarios precargados en la base de datos (`vendedor@test.com`, `comprador1@test.com`, `comprador2@test.com`, `sinfondos@test.com`) utilizan la contraseña: **`123456`**.


---
## 📡 Endpoints de la API REST

### Subastas (`/api/auctions`)
| Método | Endpoint | Descripción | Códigos de Estado HTTP |
|:---:|---|---|---|
| **GET** | `/api/auctions` | Catálogo con filtros por estado, categoría, comprador, vendedor, búsqueda libre (`q`), ordenamiento (`orderBy`, `order`) y paginación server-side (`page`, `pageSize`). | `200 OK` |
| **GET** | `/api/auctions/{id}` | Detalle completo de la subasta, estado y puja líder. | `200 OK`, `404 Not Found` |
| **POST** | `/api/auctions` | Creación de nueva subasta con validación de fechas y precios. | `201 Created`, `422 Unprocessable` |
| **POST** | `/api/auctions/{id}/bids` | Registro de puja (evalúa saldo, ejecuta Escrow atómico y Anti-Sniping). | `200 OK`, `409 Conflict`, `422 Unprocessable` |
| **GET** | `/api/auctions/{id}/bids` | Historial cronológico de pujas anonimizadas para la sala en vivo. | `200 OK`, `404 Not Found` |

### Actividades de Usuario (`/api/users/{usuarioId}`)
| Método | Endpoint | Descripción | Códigos de Estado HTTP |
|:---:|---|---|---|
| **GET** | `/api/users/{usuarioId}/bids` | Consulta dedicada de subastas donde el usuario participó ("Mis Pujas"). | `200 OK`, `404 Not Found` |
| **GET** | `/api/users/{usuarioId}/auctions` | Consulta dedicada de subastas creadas por el usuario ("Mis Publicaciones"). | `200 OK`, `404 Not Found` |

### Billetera Virtual (`/api/wallets`)
| Método | Endpoint | Descripción | Códigos de Estado HTTP |
|:---:|---|---|---|
| **GET** | `/api/wallets/balance?usuarioId={id}` | Desglose de saldos: Total, Retenido en Escrow y Disponible. | `200 OK`, `404 Not Found` |
| **GET** | `/api/wallets/transactions?usuarioId={id}` | Historial de movimientos financieros. Permite consultar detalle de ingresos, retenciones por ofertas, liberaciones y débitos finales por subastas ganadas | `200 OK` |
| **POST** | `/api/wallets/deposit` | Acreditación simulada de fondos con asiento en Ledger y auditoría. | `200 OK`, `400 Bad Request` |

### Autenticación (`/api/auth`)
| Método | Endpoint | Descripción | Códigos de Estado HTTP |
|:---:|---|---|---|
| **POST** | `/api/auth/register` | Registro de nuevos usuarios con cifrado de contraseña (BCrypt), creación automática de Billetera Virtual en $0 y emisión de token JWT. | `200 OK`, `400 Bad Request` |
| **POST** | `/api/auth/login` | Validación de credenciales (email y contraseña con BCrypt) y emisión de token de autenticación JWT. | `200 OK`, `401 Unauthorized` |

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
