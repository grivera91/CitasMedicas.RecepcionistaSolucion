using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CitasMedicas.RecepcionistaApi.Model
{
    public class Recepcionista
    {
        [Key]
        public int IdRecepcionista { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }        

        [Required]
        [MaxLength(20)]
        public string CodigoRecepcionista { get; set; }

        [Required]
        public DateTime FechaContratacion { get; set; }

        [Required]
        [MaxLength(20)]
        public string Turno { get; set; }

        [MaxLength(100)]
        public string Departamento { get; set; }
        
        [Required]
        public bool EsActivo { get; set; } = true;

        [Required]
        [MaxLength(50)]
        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? UsuarioModificacion { get; set; }

        public DateTime? FechaModificacion { get; set; }
    }
}
