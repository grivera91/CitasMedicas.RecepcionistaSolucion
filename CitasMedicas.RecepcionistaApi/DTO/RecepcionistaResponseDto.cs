namespace CitasMedicas.RecepcionistaApi.DTO
{
    public class RecepcionistaResponseDto
    {
        public int IdRecepcionista { get; set; }
        public int IdUsuario { get; set; }
        public string CodigoRecepcionista { get; set; }
        public DateTime FechaContratacion { get; set; }
        public string Turno { get; set; }
        public string Departamento { get; set; }        
        public bool EsActivo { get; set; }        
    }
}