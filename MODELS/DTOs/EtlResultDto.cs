namespace gatitosEtl.Models.DTOs
{
    public class EtlResultDto
    {
        public bool Success { get; set; }
        public int PersonasProcessadas { get; set; }
        public int GatosProcessados { get; set; }
        public int CiudadesProcessadas { get; set; }
        public int FechasProcessadas { get; set; }
        public List<string> Errores { get; set; } = new();
        public string Mensaje { get; set; } = string.Empty;
    }
}
