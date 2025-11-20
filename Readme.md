
# 📄 Document Validation API

API backend para la gestión y validación de documentos, implementada en **ASP.NET Core 8** con arquitectura hexagonal (Api / Application / Domain / Infrastructure) y desplegada en **AWS Lambda**, usando **Azure SQL Database** para metadatos y **Amazon S3** para almacenamiento mediante **pre-signed URLs**.

---

# 🏗️ 1. Estructura del Proyecto

```
DocumentValidationAPI/
├─ DocumentValidationAPI.Api/             # Controllers, Filters, Swagger, JWT, Startup
├─ DocumentValidationAPI.Application/     # UseCases, Services, DTOs, Abstractions
├─ DocumentValidationAPI.Domain/          # Entities, ValueObjects, Enums, Ports
└─ DocumentValidationAPI.Infrastructure/  # DbContext, Migrations, Repositories, S3
```

---

# 🧱 2. Modelos del Dominio

### **Company**
Empresa propietaria de documentos.

### **BusinessEntity**
Entidad asociada a la empresa (p. ej. vehículo).

### **Document**
Metadatos del archivo almacenado en S3:
- `BucketKey`
- `MimeType`
- `ValidationStatus`
- `CreatedAt`, `UpdatedAt`

### **ValidationStep**
Define el flujo de aprobación:
- `Order`
- `ApproverUserId`
- `Status` (`P`, `A`, `R`)
- `ApprovedAt`

### **DocumentAction**
Historial de acciones:
- `UPLOAD`, `DOWNLOAD`, `APPROVE`, `REJECT`

---

# 🎯 3. Casos de Uso

Ubicación:  
`DocumentValidationAPI.Application/UseCases/Documents/`

### ✔ UploadDocument
- Valida entrada
- Crea documento en DB
- Genera pre-signed URL PUT
- Registra acción "UPLOAD"

### ✔ GetDownloadUrl
- Genera pre-signed URL GET
- Registra acción "DOWNLOAD"

### ✔ ApproveDocument
Reglas:
- Aprobador puede aprobar su step
- Si es de mayor orden, aprueba también los anteriores
- Si todos quedan aprobados → documento `"A"`
- Registra acción "APPROVE"

### ✔ RejectDocument
- Valida que el usuario pertenezca al flujo
- Rechaza todos los steps y documento
- Registra acción "REJECT"

---

# 📦 4. Cliente de Storage (Amazon S3)

### **Interfaz (`IStorageService`)**
```csharp
string GenerateUploadUrl(string bucketKey, string mimeType, long sizeBytes);
string GenerateDownloadUrl(string bucketKey);
```

### **Implementación: `S3StorageService`**
- Genera URLs pre-firmadas PUT y GET
- Define expiración `ExpiresMinutes`
- Región y bucket desde env vars

---

# 🗃️ 5. Migraciones (EF Core)

### Crear migración
```bash
dotnet ef migrations add InitialCreate   -p DocumentValidationAPI.Infrastructure   -s DocumentValidationAPI.Api
```

### Aplicar migraciones
```bash
dotnet ef database update   -p DocumentValidationAPI.Infrastructure   -s DocumentValidationAPI.Api
```

---

# 🔧 6. Variables de Entorno

## appsettings.json (Local)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:...;Initial Catalog=document_db;..."
  },
  "Storage": {
    "BucketName": "document-companies",
    "Region": "us-east-1",
    "ExpiresMinutes": 10
  },
  "Jwt": {
    "Issuer": "DocumentValidationAPI",
    "Audience": "DocumentValidationAPI",
    "Key": "dev-secret"
  }
}
```

## AWS Lambda (Producción)

```
ConnectionStrings__DefaultConnection
Storage__BucketName
Storage__Region
Storage__ExpiresMinutes
Jwt__Issuer
Jwt__Audience
Jwt__Key
```

---

# 🚀 7. Levantar el Proyecto (Local)

### 1. Restaurar dependencias
```bash
dotnet restore
```

### 2. Ejecutar migraciones
```bash
dotnet ef database update -p DocumentValidationAPI.Infrastructure -s DocumentValidationAPI.Api
```
### 3. Ejecutar pruebas

```bash
dotnet test
```

### 4. Ejecutar API
```bash
dotnet run --project DocumentValidationAPI.Api
```

Swagger:
```
http://localhost:5000/swagger
```

---

# ☁️ 8. Despliegue en AWS Lambda

### Handler
```
DocumentValidationAPI.Api::DocumentValidationAPI.Api.LambdaEntryPoint::FunctionHandlerAsync
```

### Pasos:
1. Publish to AWS Lambda desde Visual Studio
2. Seleccionar bucket para despliegue
3. Configurar env vars
4. Aplicar cambios en API Gateway

---

# ⚠️ Important Testing Warning

Por motivos de pruebas, en la base de datos solo están disponibles los siguientes registros.:

- **CompanyId:** `B8A4C5C9-7C2E-4C3A-9E2D-9D7D1F6F2E81`
- **EntityId:** `F3E1A2B7-91C4-4D8A-8F3F-2DB1E6B4C92E`
- **Entity Type:** `vehicle`

Cualquier solicitud que utilice valores diferentes a estos dará lugar a errores de validación durante las pruebas en el entorno correspondiente.

---

# 🧪 Colección de Postman para pruebas

Para facilitar el proceso de pruebas y verificación de los endpoints expuestos por este servicio, se habilita un enlace de descarga directa a una colección de Postman previamente configurada.

Esta colección incluye:

- **Rutas de ejemplo para cada endpoint.**
- **Variables de entorno sugeridas.**
- **Ejemplos de peticiones con los parámetros necesarios.**
- **Estructuras de respuesta esperadas.**

Puedes descargar la colección desde el siguiente enlace:

👉 [Descargar colección de Postman](https://drive.google.com/file/d/1UHfNl-FJVeDiwK9GQeGPDetZyRl3oZZN/view?usp=sharing)

Una vez descargada, impórtala en tu Postman desde:
File → Import → Upload Files.

---

# 🧪 9. Ejemplos de Requests

Para facilidad de pruebas se puede cambiar el ambiente local por el desplegado.
```bash
https://z8twl1q1m2.execute-api.us-east-1.amazonaws.com/Prod
```

### 🔑 Obtener JWT (para pruebas)
```bash
curl -X POST https://localhost:5000/api/auth/login   -H "Content-Type: application/json"
-d
'{
  "username": "admin",
  "password": "admin"
}'
```

### 📤 Subir documento
```bash
curl -X POST https://localhost:5000/api/documents   -H "Authorization: Bearer <token>"   -H "Content-Type: application/json"
-d
'{
  "company_id": "b8a4c5c9-7c2e-4c3a-9e2d-9d7d1f6f2e81",
  "entity": {
    "entity_type": "vehicle",
    "entity_id": "f3e1a2b7-91c4-4d8a-8f3f-2db1e6b4c92e"
  },
  "document": {
    "name": "soat.pdf",
    "mime_type": "application/pdf",
    "size_bytes": 123456,
    "bucket_key": "companies/b8a4c5c9-7c2e-4c3a-9e2d-9d7d1f6f2e81/vehicles/f3e1a2b7-91c4-4d8a-8f3f-2db1e6b4c92e/docs/soat-2025.pdf"
  },
  "validation_flow": {
    "enabled": true,
    "steps": [
      { "order": 1, "approver_user_id": "e91a7e4c-4f7f-46ac-b5f0-0c9e5de41f8b" },
      { "order": 2, "approver_user_id": "5c72d8a1-6e24-4e8b-9cd1-32b93f0a2ab2" },
      { "order": 3, "approver_user_id": "a3d9b1c2-87de-4c5e-920b-6f2db417e3c4" }
    ]
  }
}'
```

### 📥 Descargar
```bash
curl -X GET https://localhost:5000/api/documents/<id>/download   -H "Authorization: Bearer <token>"
```

### ✔ Aprobar
```bash
curl -X POST https://localhost:5000/api/documents/<id>/approve   -H "Authorization: Bearer <token>"
-d
'{
  "actor_user_id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "reason": "OK"
}'
```

### ❌ Rechazar
```bash
curl -X POST https://localhost:5000/api/documents/<id>/reject   -H "Authorization: Bearer <token>"   
-d
'{
  "actor_user_id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "reason": "OK"
}'
```

---
