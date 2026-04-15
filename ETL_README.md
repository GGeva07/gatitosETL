# Sistema ETL gatitosETL

## Descripción General

Este es un sistema ETL (Extract, Transform, Load) desarrollado en .NET 8 que procesa datos de un archivo CSV y los carga en una base de datos de dimensiones (data warehouse).

## Arquitectura

### Componentes Principales

1. **CsvReaderService** (`ICsvReaderService`)
   - Responsable de leer archivos CSV
   - Parsea las líneas CSV respetando comillas
   - Mapea los datos a DTOs (`CsvDataDto`)

2. **EtlService** (`IEtlService`)
   - Orquesta el proceso de transformación y carga
   - Procesa ciudades, personas, gatos y fechas
   - Valida datos y previene duplicados
   - Maneja relaciones entre entidades

3. **Repositorios Genéricos**
   - `GenericRepository<T>`: Implementa operaciones CRUD básicas
   - Especializados: `CiudadRepository`, `PersonaRepository`, `GatoRepository`, `FechaRepository`
   - Todos heredan de `GenericRepository<T>` con métodos async/await

4. **DbContext**
   - `DbGatitosContext`: Contexto de Entity Framework Core
   - Define DbSets para DimCiudad, DimPersona, DimGato, DimFecha
   - Configuración de claves primarias

### Entidades (Dimensiones)

#### DimCiudad
```csharp
- id_ciudad (PK)
- nombre (string)
```

#### DimPersona
```csharp
- id_persona (PK)
- nombre (string)
- idCiudad (FK -> DimCiudad)
```

#### DimGato
```csharp
- id_gato (PK)
- nombre (string)
- raza (string)
- tipo (string)
```

#### DimFecha
```csharp
- id_fecha (PK)
- dia (int)
- mes (int)
- anio (int)
```

## Formato del CSV

El archivo CSV debe tener las siguientes columnas:

```
id_persona,nombre_persona,ciudad,id_gato,nombre_gato,raza,tipo,fecha
```

### Ejemplo:
```csv
1,Juan Pérez,Madrid,101,Whiskers,Persa,Doméstico,2024-01-15
2,María García,Barcelona,102,Fluffy,Siamés,Doméstico,2024-02-20
3,Carlos López,Valencia,103,Mittens,Gato Común,Doméstico,2024-03-10
```

## Flujo de Procesamiento ETL

1. **Lectura (Extract)**
   - Se lee el archivo CSV especificado
   - Se parseann las líneas respetando formato CSV

2. **Transformación (Transform)**
   - **Ciudades**: Se extraen ciudades únicas y se validan
   - **Personas**: Se procesan personas únicas, se validan IDs y se asocian con ciudades
   - **Gatos**: Se procesan gatos únicos, se validan IDs
   - **Fechas**: Se parsean fechas y se descomponen en día/mes/año

3. **Carga (Load)**
   - Se verifica existencia previa (duplicados)
   - Se insertan nuevos registros
   - Se consolidan cambios en la base de datos

## Configuración

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DWH": "Server=DESKTOP-A2N4IGH;Database=dbgatitos;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Warning"
    }
  }
}
```

Actualiza la cadena de conexión según tu entorno.

## Ejecución

### Opción 1: Ejecutar el Worker Service
```bash
cd WorkerService
dotnet run
```

### Opción 2: Desde la carpeta raíz
```bash
dotnet run --project WorkerService/gatittosEtl.WorkerService.csproj
```

## Servicios de Negocio

### CiudadService
```csharp
- GetAllAsync(): Obtiene todas las ciudades
- GetByIdAsync(id): Obtiene ciudad por ID
- ExistsCiudadAsync(nombre): Verifica existencia
- AddCiudadAsync(ciudad): Agrega nueva ciudad
```

### PersonaService
```csharp
- GetAllAsync(): Obtiene todas las personas
- GetByIdAsync(id): Obtiene persona por ID
- GetByNombreAsync(nombre): Busca por nombre
- GetByCiudadAsync(idCiudad): Obtiene personas de una ciudad
- AddPersonaAsync(persona): Agrega nueva persona
- UpdatePersonaAsync(persona): Actualiza persona
- DeletePersonaAsync(persona): Elimina persona
```

### GatoService
```csharp
- GetAllAsync(): Obtiene todos los gatos
- GetByIdAsync(id): Obtiene gato por ID
- GetByNombreAsync(nombre): Busca por nombre
- GetByRazaAsync(raza): Obtiene gatos de una raza
- GetByTipoAsync(tipo): Obtiene gatos de un tipo
- AddGatoAsync(gato): Agrega nuevo gato
- UpdateGatoAsync(gato): Actualiza gato
- DeleteGatoAsync(gato): Elimina gato
```

### FechaService
```csharp
- GetAllAsync(): Obtiene todas las fechas
- GetByIdAsync(id): Obtiene fecha por ID
- GetByAnioAsync(anio): Obtiene fechas de un año
- GetByMesAsync(mes): Obtiene fechas de un mes
- GetByRangoAsync(desde, hasta): Obtiene rango de fechas
- AddFechaAsync(fecha): Agrega nueva fecha
- UpdateFechaAsync(fecha): Actualiza fecha
- DeleteFechaAsync(fecha): Elimina fecha
```

## Manejo de Errores

El sistema captura y reporta:
- Archivos no encontrados
- Formatos de CSV inválidos
- Conversiones de tipos fallidas
- Errores de base de datos
- Duplicados detectados

Los errores se registran en los logs y en el resultado del ETL (`EtlResultDto`).

## DTOs

### CsvDataDto
Mapeo de datos leídos del CSV

### EtlResultDto
Resultado del procesamiento ETL
```csharp
- Success (bool): Éxito del procesamiento
- PersonasProcessadas (int)
- GatosProcessados (int)
- CiudadesProcessadas (int)
- FechasProcessadas (int)
- Errores (List<string>): Lista de errores
- Mensaje (string): Mensaje descriptivo
```

## Dependencias Instaladas

- `Microsoft.EntityFrameworkCore` (8.0.11)
- `Microsoft.EntityFrameworkCore.SqlServer` (8.0.11)
- `CsvHelper` (33.1.0) [Opcional - Actualmente no se usa]

## Próximas Mejoras

- [ ] Integración con base de datos Azure
- [ ] Procesamiento de archivos en lotes
- [ ] Notificaciones de éxito/error
- [ ] Dashboard de monitoreo
- [ ] Soporte para múltiples formatos de entrada
- [ ] Validaciones más robustas
- [ ] Auditoría de cambios

## Autor

Sistema ETL para el proyecto gatitosETL - 2024
