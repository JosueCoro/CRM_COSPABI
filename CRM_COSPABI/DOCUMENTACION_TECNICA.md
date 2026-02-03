# Documentación Técnica - CRM COSPABI Backend

## 1. Visión General
Este proyecto es el backend para el sistema CRM de la Cooperativa de Agua Potable COSPABI. Gestiona clientes, lecturas de medidores, facturación automática y pagos en línea mediante la pasarela Libélula.

**Tecnologías:**
- .NET 8/9 Web API
- Entity Framework Core (Database First)
- SQL Server
- JWT Authentication

---

## 2. Arquitectura del Proyecto

El proyecto sigue una arquitectura en capas:

- **Controllers**: Puntos de entrada HTTP. Manejan la seguridad y validan la entrada.
- **Services (Lógica de Negocio)**: Contienen toda la lógica (cálculos, reglas, integraciones). Implementan interfaces para inyección de dependencias.
- **Models (Data)**: Clases generadas por EF Core que mapean directamente a la Base de Datos.
- **DTOs**: Objetos de transferencia de datos para desacoplar los modelos de base de datos de la API pública.

### Estructura de Carpetas Clave

| Carpeta | Descripción |
|---------|-------------|
| `/Controllers` | Endpoints de la API (`LecturaController`, `PagoController`, `AuthController`). |
| `/Service` | Lógica principal (`LecturaService`, `PagoService`, `AuthService`). |
| `/DTOs` | Modelos de entrada/salida (`IniciarPagoDto`, `LecturaCreateDto`). |
| `/Models` | Entidades de BD (`Factura`, `Lectura`, `Cliente`). |

---

## 3. Módulos Principales

### 3.1. Autenticación y Seguridad
El sistema utiliza **JWT (JSON Web Tokens)**.
Existen dos flujos de login distintos que generan tokens con Claims específicos:
1.  **Administrativos** (`/api/auth/login/admin`): Para roles `ADMIN`, `LECTOR`, `CAJERO`.
2.  **Clientes** (`/api/auth/login/cliente`): Para roles `CLIENTE_SOCIO`, `CLIENTE_CONSUMIDOR`.

**Claims Clave en el Token:**
- `NameIdentifier`: ID del usuario (UsuarioAdmin o Cliente).
- `Role`: Rol del usuario (ej. `"ADMIN"`, `"CLIENTE_SOCIO"`).
- `Permisos`: Lista de códigos de permiso (ej. `["G_LECTURA", "G_PAGO"]`).

### 3.2. Módulo de Lecturas y Facturación
**Flujo Automático:**
1.  El `LECTOR` envía una lectura (`POST /api/lectura`).
2.  `LecturaService` calcula el consumo: `LecturaActual - LecturaAnterior`.
3.  **Generación de Factura**:
    - Se crea automáticamente una Factura pendiente.
    - Cálculo: `(Consumo * 1.66 Bs) + 15 Bs (Tarifa Base)`.
    - Se guardan los ítems en `DetalleFactura`.

### 3.3. Pasarela de Pagos (Libélula)
Integración backend-to-backend para seguridad.

- **Iniciar Pago** (`POST /api/pago/iniciar`):
    - Recibe `IdFactura`.
    - Crea una "Deuda" en la API de Libélula usando la `AppKey`.
    - Retorna la URL y QR de pago al frontend.
- **Callback/Webhook** (`POST /api/pago/callback`):
    - Endpoint público que Libélula llama al confirmarse el pago.
    - Valida la transacción.
    - Actualiza factura a `PAGADA`.
    - Crea registros en `Pago` y `ComprobantePago`.

---

## 4. Configuración (appsettings.json)

Se requiere configurar la sección de Libélula y JWT:

```json
"Libelula": {
  "BaseUrl": "https://api.libelula.bo/rest",
  "AppKey": "TU_APP_KEY_AQUI",
  "CallbackUrl": "https://tudominio.com/api/pago/callback",
  "ReturnUrl": "https://tudominio.com/cliente/pagos/resultado"
},
"Jwt": {
  "Key": "CLAVE_SECRETA_MUY_LARGA...",
  "Issuer": "...",
  "Audience": "..."
}
```

## 5. Guía para Desarrolladores

### Cómo agregar un nuevo módulo
1.  Crear la Entidad en DB y actualizar Modelos (si aplica).
2.  Crear DTOs en `/DTOs`.
3.  Crear Interface `IService` y su implementación `Service`.
4.  Registrar el servicio en `Program.cs` (`builder.Services.AddScoped<I..., ...>`).
5.  Crear el `Controller` e inyectar el servicio.

### Notas Importantes
- **Lecturas**: No se permite editar una lectura si ya generó una factura pagada.
- **Fechas**: Usamos `DateOnly` para fechas sin hora (Vencimientos, Lecturas).

---
*Generado automáticamente por Antigravity AI - 2026*
