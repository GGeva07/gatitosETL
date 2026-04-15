using gatitosEtl.Models.DTOs;

namespace gatitosEtl.Data.interfaces
{
    public interface ICsvReaderService
    {
        Task<List<CsvDataDto>> ReadCsvAsync(string filePath);
    }
}
