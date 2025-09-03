using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotoApi.Models
{
    [Table("Moto")]
    public class Moto
    {
        [Key]
        [Column("id_moto")]
        public int IdMoto { get; set; }

        [Required]
        [Column("modelo")]
        [MaxLength(50)]
        public required string Modelo { get; set; } = string.Empty;

        [Required]
        [Column("placa")]
        [MaxLength(7)]
        public required string Placa { get; set; } = string.Empty;

        [Column("statusmoto_id_status")]
        public int StatusMotoId { get; set; }
    }
}
