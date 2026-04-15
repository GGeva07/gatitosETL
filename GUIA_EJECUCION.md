# ?? GUÍA DE EJECUCIÓN - Sistema ETL gatitosETL

## ? Estado Actual
- ? **Compilación exitosa**
- ? **Todas las dependencias instaladas**
- ? **Configuración lista**
- ? **Base de datos configurada**

## ?? Pasos para Ejecutar

### Paso 1: Preparar la Base de Datos

#### Opción A: Con Entity Framework Core (Recomendado - Automático)
```bash
cd Data
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Opción B: Script SQL Manual
1. Abre SQL Server Management Studio
2. Crea una base de datos llamada `dbgatitos`
3. Ejecuta el script: `SQL_SCHEMA.sql`

### Paso 2: Verificar Configuración

Abre `WorkerService\appsettings.json` y verifica:

```json
{
  "ConnectionStrings": {
    "DWH": "Server=DESKTOP-A2N4IGH;Database=dbgatitos;TrustServerCertificate=True;"
  }
}
```

?? **Cambia `DESKTOP-A2N4IGH` por tu servidor SQL Server**

### Paso 3: Preparar el Archivo CSV

El archivo CSV debe estar en: `WorkerService\data\datos.csv`

Formato esperado:
```
id_persona,nombre_persona,ciudad,id_gato,nombre_gato,raza,tipo,fecha
1,Juan Pérez,Madrid,101,Whiskers,Persa,Doméstico,2024-01-15
2,María García,Barcelona,102,Fluffy,Siamés,Doméstico,2024-02-20
```

? Ya hay un archivo de ejemplo en ese directorio.

### Paso 4: Ejecutar el Worker Service

```bash
cd WorkerService
dotnet run
```

O desde la raíz:
```bash
dotnet run --project WorkerService/gatittosEtl.WorkerService.csproj
```

## ?? Salida Esperada

```
info: Worker[0]
      Worker iniciado - Procesando ETL
info: Worker[0]
      Leyendo CSV desde: data/datos.csv
info: Worker[0]
      Se leyeron 5 registros del CSV
info: Worker[0]
      Iniciando procesamiento ETL
info: Worker[0]
      ETL completado exitosamente: Ciudades=5, Personas=5, Gatos=5, Fechas=5
```

## ?? Verificar Resultados

Después de ejecutar, verifica en la base de datos:

```sql
-- Contar registros
SELECT 'Ciudades' as Tabla, COUNT(*) FROM DimCiudades
UNION ALL
SELECT 'Personas', COUNT(*) FROM DimPersonas
UNION ALL
SELECT 'Gatos', COUNT(*) FROM DimGatos
UNION ALL
SELECT 'Fechas', COUNT(*) FROM DimFechas;

-- Ver los datos insertados
SELECT * FROM DimCiudades;
SELECT * FROM DimPersonas;
SELECT * FROM DimGatos;
SELECT * FROM DimFechas;
```

## ?? Solución de Problemas

### Error: "El archivo CSV no encontrado"
```
? Asegúrate que existe: WorkerService/data/datos.csv
? Verifica la ruta exacta
```

### Error: "Connection timeout"
```
? Verifica que tu SQL Server esté ejecutándose
? Comprueba el nombre del servidor en appsettings.json
? Verifica que la base de datos exista
```

### Error: "Database does not exist"
```
? Crea la base de datos: dbgatitos
? O ejecuta las migraciones de EF Core
```

### Error: "The type or namespace name could not be found"
```
? Ejecuta: dotnet restore
? Cierra y reabre Visual Studio
```

## ?? Estructura de Archivos Importante

```
gatitosETL/
??? WorkerService/
?   ??? Program.cs           # Configuración DI
?   ??? Worker.cs            # BackgroundService ETL
?   ??? appsettings.json     # Configuración cadena conexión
?   ??? data/
?       ??? datos.csv        # Archivo CSV entrada
??? Data/
?   ??? context/
?   ?   ??? dbGatitosContext.cs
?   ??? interfaces/
?   ?   ??? ICsvReaderService.cs
?   ?   ??? IEtlService.cs
?   ?   ??? [Repositorios]
?   ??? repo/
?   ?   ??? GenericRepository.cs
?   ?   ??? [Repositorios especializados]
?   ??? services/
?       ??? CsvReaderService.cs
?       ??? EtlService.cs
?       ??? [Servicios de negocio]
??? MODELS/
    ??? DIMS/
    ?   ??? DimCiudad.cs
    ?   ??? DimPersona.cs
    ?   ??? DimGato.cs
    ?   ??? DimFecha.cs
    ??? DTOs/
        ??? CsvDataDto.cs
        ??? EtlResultDto.cs
```

## ?? Flujo de Datos

```
datos.csv (input)
    ? [CsvReaderService]
CsvDataDto[] (datos parseados)
    ? [EtlService]
  ?? DimCiudades (insert/update)
  ?? DimPersonas (insert/update)
  ?? DimGatos (insert/update)
  ?? DimFechas (insert/update)
    ? [DbContext.SaveChangesAsync()]
dbgatitos database
```

## ?? Información de Conexión SQL Server

**Valores por defecto en appsettings.json:**
- **Servidor**: DESKTOP-A2N4IGH (cambiar por el tuyo)
- **Base de datos**: dbgatitos
- **Autenticación**: Integrada (Windows)
- **TrustServerCertificate**: True

## ??? Comandos Útiles

```bash
# Limpiar y reconstruir
dotnet clean
dotnet build

# Ejecutar solo el proyecto WorkerService
cd WorkerService && dotnet run

# Restaurar paquetes
dotnet restore

# Crear migraciones EF Core
dotnet ef migrations add MigrationName
dotnet ef database update

# Ver logs detallados
# Configurar en appsettings.json: "Default": "Debug"
```

## ?? Soporte

Si tienes problemas:

1. ? Verifica compilación: `dotnet build`
2. ? Verifica configuración: `appsettings.json`
3. ? Verifica base de datos: SQL Server Management Studio
4. ? Verifica archivo CSV: `WorkerService/data/datos.csv`
5. ? Revisa logs de salida

## ? Próximos Pasos

Después de ejecutar exitosamente:

1. **Explorar datos**: Ejecuta queries SQL en `SQL_SCHEMA.sql`
2. **Crear API REST**: Agrega controladores para exponer servicios
3. **Monitoreo**: Implementa dashboard
4. **Procesamiento automático**: Configura ejecución programada
5. **Validaciones adicionales**: Amplía reglas de negocio

---

**¡Sistema ETL listo para usar! ??**
