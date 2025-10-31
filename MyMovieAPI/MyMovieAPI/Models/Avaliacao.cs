using System.ComponentModel.DataAnnotations.Schema;

namespace MyMovieAPI.Models;

public class Avaliacao : EntidadeBase
{
    public double Nota { get; set; }
    public int Popularidade { get; set; }
    public int QtdeVotos { get; set; }
    [ForeignKey("Filme")]
    public int FilmeId { get; set; }
}
