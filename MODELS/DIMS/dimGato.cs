namespace gatitosEtl.Models.DIMS
{
    public class DimGato
    {
        public int id_gato { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string raza { get; set; } = string.Empty;
        public string tipo { get; set; } = string.Empty;
    }
}