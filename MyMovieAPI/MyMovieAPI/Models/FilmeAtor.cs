using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovieAPI.Models;    

public class FilmeAtor : EntidadeBase
{
    [ForeignKey("Filme")]
    public int FilmeId { get; set; }

    [ForeignKey("Ator")]
    public int AtorId { get; set; }
}
