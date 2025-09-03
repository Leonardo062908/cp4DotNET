using System.ComponentModel.DataAnnotations;

namespace MotoApi.Dtos;

public record MotoResponseDto(int IdMoto, string Modelo, string Placa, int StatusMotoId);

public class MotoCreateDto
{
    [Required, StringLength(50)]
    public required string Modelo { get; init; }

    // Placa Mercosul: ABC1D23
    [Required, RegularExpression(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$")]
    [StringLength(7, MinimumLength = 7)]
    public required string Placa { get; init; }

    [Range(0, int.MaxValue)]
    public int StatusMotoId { get; init; }
}

public class MotoUpdateDto : MotoCreateDto { }