using System.ComponentModel.DataAnnotations;

namespace MyMovieAPI.Models;

public class Ator : EntidadeBase
{
    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    public DateTime DataNascimento { get; set; }

    [StringLength(50)]
    public string Nacionalidade { get; set; }
}
