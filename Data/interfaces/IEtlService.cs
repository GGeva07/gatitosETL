using gatitosEtl.Models.DTOs;

namespace gatitosEtl.Data.interfaces
{
    public interface IEtlService
    {
        Task<EtlResultDto> ProcessEtlAsync(List<CsvDataDto> csvData);
    }
}
