-- ============================================================================
-- SCRIPT SQL PARA CREAR LAS TABLAS DEL DATA WAREHOUSE
-- ============================================================================
-- Esta base de datos se crearà automàticamente por Entity Framework Core
-- cuando se ejecute el migration, pero puedes usar este script como referencia
-- ============================================================================

USE dbgatitos;
GO

-- ============================================================================
-- TABLA: DimCiudad
-- Dimensión de ciudades
-- ============================================================================

IF OBJECT_ID('dbo.DimCiudades', 'U') IS NOT NULL 
    DROP TABLE dbo.DimCiudades;
GO

CREATE TABLE dbo.DimCiudades (
    id_ciudad INT PRIMARY KEY IDENTITY(1,1),
    nombre NVARCHAR(255) NOT NULL,
    CONSTRAINT UQ_DimCiudades_nombre UNIQUE(nombre)
);

-- ============================================================================
-- TABLA: DimFecha
-- Dimensión de fechas (descompuesta)
-- ============================================================================

IF OBJECT_ID('dbo.DimFechas', 'U') IS NOT NULL 
    DROP TABLE dbo.DimFechas;
GO

CREATE TABLE dbo.DimFechas (
    id_fecha INT PRIMARY KEY IDENTITY(1,1),
    dia INT NOT NULL,
    mes INT NOT NULL,
    anio INT NOT NULL,
    CONSTRAINT CK_DimFechas_dia CHECK(dia >= 1 AND dia <= 31),
    CONSTRAINT CK_DimFechas_mes CHECK(mes >= 1 AND mes <= 12),
    CONSTRAINT CK_DimFechas_anio CHECK(anio > 1900),
    CONSTRAINT UQ_DimFechas_fecha UNIQUE(dia, mes, anio)
);

-- ============================================================================
-- TABLA: DimPersona
-- Dimensión de personas
-- ============================================================================

IF OBJECT_ID('dbo.DimPersonas', 'U') IS NOT NULL 
    DROP TABLE dbo.DimPersonas;
GO

CREATE TABLE dbo.DimPersonas (
    id_persona INT PRIMARY KEY,
    nombre NVARCHAR(255) NOT NULL,
    idCiudad INT NOT NULL,
    CONSTRAINT FK_DimPersonas_DimCiudades FOREIGN KEY(idCiudad) 
        REFERENCES dbo.DimCiudades(id_ciudad)
        ON DELETE CASCADE
);

CREATE INDEX IX_DimPersonas_nombre ON dbo.DimPersonas(nombre);
CREATE INDEX IX_DimPersonas_idCiudad ON dbo.DimPersonas(idCiudad);

-- ============================================================================
-- TABLA: DimGato
-- Dimensión de gatos
-- ============================================================================

IF OBJECT_ID('dbo.DimGatos', 'U') IS NOT NULL 
    DROP TABLE dbo.DimGatos;
GO

CREATE TABLE dbo.DimGatos (
    id_gato INT PRIMARY KEY,
    nombre NVARCHAR(255) NOT NULL,
    raza NVARCHAR(100),
    tipo NVARCHAR(100)
);

CREATE INDEX IX_DimGatos_nombre ON dbo.DimGatos(nombre);
CREATE INDEX IX_DimGatos_raza ON dbo.DimGatos(raza);
CREATE INDEX IX_DimGatos_tipo ON dbo.DimGatos(tipo);

-- ============================================================================
-- CONSULTAS DE PRUEBA
-- ============================================================================

-- Ver todas las ciudades
SELECT * FROM dbo.DimCiudades;

-- Ver todas las personas con sus ciudades
SELECT p.id_persona, p.nombre, c.nombre as ciudad
FROM dbo.DimPersonas p
INNER JOIN dbo.DimCiudades c ON p.idCiudad = c.id_ciudad;

-- Ver todos los gatos
SELECT * FROM dbo.DimGatos;

-- Ver todas las fechas
SELECT * FROM dbo.DimFechas
ORDER BY anio, mes, dia;

-- Contar registros por tabla
SELECT 'DimCiudades' as Tabla, COUNT(*) as Total FROM dbo.DimCiudades
UNION ALL
SELECT 'DimPersonas', COUNT(*) FROM dbo.DimPersonas
UNION ALL
SELECT 'DimGatos', COUNT(*) FROM dbo.DimGatos
UNION ALL
SELECT 'DimFechas', COUNT(*) FROM dbo.DimFechas;

-- Personas por ciudad
SELECT c.nombre as Ciudad, COUNT(p.id_persona) as TotalPersonas
FROM dbo.DimCiudades c
LEFT JOIN dbo.DimPersonas p ON c.id_ciudad = p.idCiudad
GROUP BY c.nombre;

-- Gatos por raza
SELECT raza, COUNT(*) as Total
FROM dbo.DimGatos
GROUP BY raza;

-- Gatos por tipo
SELECT tipo, COUNT(*) as Total
FROM dbo.DimGatos
GROUP BY tipo;

-- Fechas por año
SELECT anio, COUNT(*) as Total
FROM dbo.DimFechas
GROUP BY anio;
