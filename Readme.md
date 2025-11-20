
# Document Validation API

API backend para la gestión y validación de documentos, implementada en **ASP.NET Core 8** con arquitectura hexagonal (Api / Application / Domain / Infrastructure) y despliegue en **AWS Lambda**.  
El proyecto utiliza **SQL Server (Azure)** como base de datos y **Amazon S3** como almacenamiento de documentos mediante **pre-signed URLs**.

## Estructura del Proyecto

```
DocumentValidationAPI/
├─ DocumentValidationAPI.Api/
├─ DocumentValidationAPI.Application/
├─ DocumentValidationAPI.Domain/
└─ DocumentValidationAPI.Infrastructure/
```

## Modelos Principales

- Company
- BusinessEntity
- Document
- ValidationStep
- DocumentAction

## Casos de Uso

- UploadDocument
- GetDownloadUrl
- ApproveDocument
- RejectDocument
- RegisterAction

## Cliente de Storage (S3)

Interfaz:
```
IStorageService
```

Implementación:
```
S3StorageService
```

## Migraciones (EF Core)

Crear migración:
```
dotnet ef migrations add InitialCreate -p DocumentValidationAPI.Infrastructure -s DocumentValidationAPI.Api
```

Aplicar migraciones:
```
dotnet ef database update -p DocumentValidationAPI.Infrastructure -s DocumentValidationAPI.Api
```

## Variables de Entorno

### appsettings.json (local)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Storage": {
    "BucketName": "",
    "Region": "",
    "ExpiresMinutes": 10
  },
  "Jwt": {
    "Issuer": "",
    "Audience": "",
    "Key": ""
  }
}
```

### AWS Lambda

```
ConnectionStrings__DefaultConnection
Storage__BucketName
Storage__Region
Storage__ExpiresMinutes
Jwt__Issuer
Jwt__Audience
Jwt__Key
```

## Levantar el Proyecto

```
dotnet restore
dotnet ef database update -p DocumentValidationAPI.Infrastructure -s DocumentValidationAPI.Api
dotnet run --project DocumentValidationAPI.Api
```

Swagger:
```
http://localhost:5000/swagger
```

## Despliegue en AWS Lambda

Handler:
```
DocumentValidationAPI.Api::DocumentValidationAPI.Api.LambdaEntryPoint::FunctionHandlerAsync
```

## Ejemplos de Requests

### Subir documento

```bash
curl -X POST https://localhost:5000/api/documents ...
```

### Aprobar documento

```bash
curl -X POST https://localhost:5000/api/documents/{id}/approve ...
```

### Rechazar documento

```bash
curl -X POST https://localhost:5000/api/documents/{id}/reject ...
```


