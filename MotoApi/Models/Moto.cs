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
        public string Modelo { get; set; }

        [Required]
        [Column("placa")]
        [MaxLength(7)]
        public string Placa { get; set; }

        [Column("statusmoto_id_status")]
        public int StatusMotoId { get; set; }
    }
}
