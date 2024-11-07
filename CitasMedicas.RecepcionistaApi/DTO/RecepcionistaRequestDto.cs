namespace CitasMedicas.RecepcionistaApi.DTO
{
    public class RecepcionistaRequestDto
    {
        public int IdUsuario { get; set; }
        public DateTime FechaContratacion { get; set; }
        public string Turno { get; set; }
        public string Departamento { get; set; }        
        public bool EsActivo { get; set; }
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
}