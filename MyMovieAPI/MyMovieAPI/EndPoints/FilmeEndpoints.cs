using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using MyMovieAPI.Data;
using MyMovieAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace MyMovieAPI.EndPoints;

public static class FilmeEndpoints
{
    public static void MapFilmeEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Filme").WithTags(nameof(Filme));

        group.MapGet("/", async (MyMovieAPIContext db) =>
        {
            return await db.Filme.ToListAsync();
        })
        .WithName("GetAllFilmes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Filme>, NotFound>> (int id, MyMovieAPIContext db) =>
        {
            return await db.Filme.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Filme model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetFilmeById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Filme filme, MyMovieAPIContext db) =>
        {
            var affected = await db.Filme
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Titulo, filme.Titulo)
                    .SetProperty(m => m.AnoLancamento, filme.AnoLancamento)
                    .SetProperty(m => m.Genero, filme.Genero)
                    .SetProperty(m => m.DuracaoMinutos, filme.DuracaoMinutos)
                    .SetProperty(m => m.Sinopse, filme.Sinopse)
                    .SetProperty(m => m.ImagemUrl, filme.ImagemUrl)
                    .SetProperty(m => m.Id, filme.Id)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateFilme")
        .WithOpenApi();

        group.MapPost("/", async (Filme filme, MyMovieAPIContext db) =>
        {
            db.Filme.Add(filme);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Filme/{filme.Id}",filme);
        })
        .WithName("CreateFilme")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, MyMovieAPIContext db) =>
        {
            var affected = await db.Filme
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteFilme")
        .WithOpenApi();

        group.MapGet("/por-titulo/{titulo}",
            async Task<Results<Ok<List<Filme>>, NotFound>> ([FromRoute] string titulo, MyMovieAPIContext db) =>
            {
                var filmes = await db.Filme
                    .AsNoTracking()
                    // Busca parcial e case-insensitive (Contains)
                    .Where(f => f.Titulo.Contains(titulo))
                    .ToListAsync();

                if (!filmes.Any())
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(filmes);
            })
        .WithName("BuscarFilmesPorTitulo")
        .WithOpenApi();

        group.MapGet("/por-ano/{ano}",
            async Task<Results<Ok<List<Filme>>, NotFound>> ([FromRoute] int ano, MyMovieAPIContext db) =>
            {
                var filmes = await db.Filme
                    .AsNoTracking()
                    .Where(f => f.AnoLancamento == ano)
                    .ToListAsync();

                if (!filmes.Any())
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(filmes);
            })
        .WithName("BuscarFilmesPorAno")
        .WithOpenApi();

        group.MapGet("/por-genero/{genero}",
            async Task<Results<Ok<List<Filme>>, NotFound>> ([FromRoute] string genero, MyMovieAPIContext db) =>
            {
                var filmes = await db.Filme
                    .AsNoTracking()
                    .Where(f => f.Genero.Equals(genero))
                    .ToListAsync();

                if (!filmes.Any())
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(filmes);
            })
        .WithName("BuscarFilmesPorGenero")
        .WithOpenApi();

        group.MapGet("/buscar",
            async Task<Results<Ok<List<Filme>>, NotFound>>
            (
                [FromQuery] string? titulo,
                [FromQuery] string? genero,
                [FromQuery] int? ano,
                MyMovieAPIContext db
            ) =>
            {
                IQueryable<Filme> query = db.Filme.AsNoTracking();

                if (!string.IsNullOrEmpty(titulo))
                {
                    query = query.Where(f => f.Titulo.Contains(titulo));
                }

                if (!string.IsNullOrEmpty(genero))
                {
                    query = query.Where(f => f.Genero.Equals(genero));
                }

                if (ano.HasValue && ano.Value > 1800)
                {
                    query = query.Where(f => f.AnoLancamento == ano.Value);
                }

                var filmes = await query.ToListAsync();

                if (!filmes.Any())
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(filmes);
            })
        .WithName("BuscarFilmesPorFiltros")
        .WithOpenApi();

        group.MapGet("/mais-longo",
            async Task<Results<Ok<Filme>, NotFound>> (MyMovieAPIContext db) =>
            {
                var filme = await db.Filme
                    .AsNoTracking()
                    .OrderByDescending(f => f.DuracaoMinutos)
                    .FirstOrDefaultAsync();

                if (filme == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(filme);
            })
        .WithName("GetFilmeMaisLongo")
        .WithOpenApi();

        group.MapPost("/carga-lote",
            async Task<Results<Created<List<Filme>>, BadRequest>> (List<Filme> filmes, MyMovieAPIContext db) =>
            {
                if (filmes == null || !filmes.Any())
                {
                    return TypedResults.BadRequest();
                }

                db.Filme.AddRange(filmes);

                await db.SaveChangesAsync();

                return TypedResults.Created("/api/Filme", filmes);
            })
        .WithName("CargaLoteFilmes")
        .WithOpenApi()
        .WithSummary("Cadastrar vários filmes");
    }
}
