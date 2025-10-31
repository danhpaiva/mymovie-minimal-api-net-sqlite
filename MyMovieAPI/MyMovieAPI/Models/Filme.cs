using System.ComponentModel.DataAnnotations;

namespace MyMovieAPI.Models;

public class Filme : EntidadeBase
{
    [Required]
    [StringLength(200)]
    public string Titulo { get; set; }
    public int AnoLancamento { get; set; }

    [Required]
    [StringLength(50)]
    public string Genero { get; set; }

    public int DuracaoMinutos { get; set; }

    [StringLength(500)]
    public string Sinopse { get; set; }

    [Required]
    [StringLength(150)]
    public string ImagemUrl { get; set; }
}
