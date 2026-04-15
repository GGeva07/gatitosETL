namespace gatitosEtl.Models.DIMS
{
    public class DimPersona
    {
        public int id_persona { get; set; }
        public string nombre { get; set; } = string.Empty;
        public int idCiudad { get; set; }
    }
}