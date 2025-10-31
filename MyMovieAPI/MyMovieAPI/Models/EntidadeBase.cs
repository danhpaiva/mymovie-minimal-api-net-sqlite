using System.ComponentModel.DataAnnotations;

namespace MyMovieAPI.Models;

public abstract class EntidadeBase
{
    [Key]
    public int Id { get; set; }
}
