using gatitosEtl.Data.interfaces;
using gatitosEtl.Models.DTOs;

namespace gatitosEtl.Data.services
{
    public class CsvReaderService : ICsvReaderService
    {
        public async Task<List<CsvDataDto>> ReadCsvAsync(string filePath)
        {
            var csvDataList = new List<CsvDataDto>();

            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Archivo CSV no encontrado: {filePath}");

                var lines = await File.ReadAllLinesAsync(filePath);

                if (lines.Length < 2)
                    throw new InvalidOperationException("El archivo CSV está vacío o no contiene datos");

                // Leer encabezados
                var headers = lines[0].Split(',');

                // Mapear índices de columnas
                var idPersonaIdx = Array.IndexOf(headers, "id_persona");
                var nombrePersonaIdx = Array.IndexOf(headers, "nombre_persona");
                var ciudadIdx = Array.IndexOf(headers, "ciudad");
                var idGatoIdx = Array.IndexOf(headers, "id_gato");
                var nombreGatoIdx = Array.IndexOf(headers, "nombre_gato");
                var razaIdx = Array.IndexOf(headers, "raza");
                var tipoIdx = Array.IndexOf(headers, "tipo");
                var fechaIdx = Array.IndexOf(headers, "fecha");

                // Procesar datos
                for (int i = 1; i < lines.Length; i++)
                {
                    var values = ParseCsvLine(lines[i]);

                    var dto = new CsvDataDto
                    {
                        IdPersona = idPersonaIdx >= 0 && idPersonaIdx < values.Length ? values[idPersonaIdx]?.Trim() : null,
                        Nombre = nombrePersonaIdx >= 0 && nombrePersonaIdx < values.Length ? values[nombrePersonaIdx]?.Trim() : null,
                        Ciudad = ciudadIdx >= 0 && ciudadIdx < values.Length ? values[ciudadIdx]?.Trim() : null,
                        IdGato = idGatoIdx >= 0 && idGatoIdx < values.Length ? values[idGatoIdx]?.Trim() : null,
                        NombreGato = nombreGatoIdx >= 0 && nombreGatoIdx < values.Length ? values[nombreGatoIdx]?.Trim() : null,
                        Raza = razaIdx >= 0 && razaIdx < values.Length ? values[razaIdx]?.Trim() : null,
                        Tipo = tipoIdx >= 0 && tipoIdx < values.Length ? values[tipoIdx]?.Trim() : null,
                        Fecha = fechaIdx >= 0 && fechaIdx < values.Length ? values[fechaIdx]?.Trim() : null
                    };

                    csvDataList.Add(dto);
                }

                return csvDataList;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al leer el archivo CSV: {ex.Message}", ex);
            }
        }

        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var current = string.Empty;
            var inQuotes = false;

            foreach (var character in line)
            {
                if (character == '"')
                    inQuotes = !inQuotes;
                else if (character == ',' && !inQuotes)
                {
                    result.Add(current);
                    current = string.Empty;
                }
                else
                    current += character;
            }

            result.Add(current);
            return result.ToArray();
        }
    }
}
