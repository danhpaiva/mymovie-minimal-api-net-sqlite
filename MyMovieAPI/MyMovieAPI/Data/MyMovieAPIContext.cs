using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyMovieAPI.Models;

namespace MyMovieAPI.Data
{
    public class MyMovieAPIContext : DbContext
    {
        public MyMovieAPIContext (DbContextOptions<MyMovieAPIContext> options)
            : base(options)
        {
        }

        public DbSet<MyMovieAPI.Models.Ator> Ator { get; set; } = default!;
        public DbSet<MyMovieAPI.Models.Filme> Filme { get; set; } = default!;
        public DbSet<MyMovieAPI.Models.FilmeAtor> FilmeAtor { get; set; } = default!;
        public DbSet<MyMovieAPI.Models.Avaliacao> Avaliacao { get; set; } = default!;
    }
}
