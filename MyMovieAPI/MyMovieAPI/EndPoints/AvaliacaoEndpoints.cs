using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using MyMovieAPI.Data;
using MyMovieAPI.Models;
namespace MyMovieAPI.EndPoints;

public static class AvaliacaoEndpoints
{
    public static void MapAvaliacaoEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Avaliacao").WithTags(nameof(Avaliacao));

        group.MapGet("/", async (MyMovieAPIContext db) =>
        {
            return await db.Avaliacao.ToListAsync();
        })
        .WithName("GetAllAvaliacaos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Avaliacao>, NotFound>> (int id, MyMovieAPIContext db) =>
        {
            return await db.Avaliacao.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Avaliacao model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAvaliacaoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Avaliacao avaliacao, MyMovieAPIContext db) =>
        {
            var affected = await db.Avaliacao
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Nota, avaliacao.Nota)
                    .SetProperty(m => m.Popularidade, avaliacao.Popularidade)
                    .SetProperty(m => m.QtdeVotos, avaliacao.QtdeVotos)
                    .SetProperty(m => m.FilmeId, avaliacao.FilmeId)
                    .SetProperty(m => m.Id, avaliacao.Id)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAvaliacao")
        .WithOpenApi();

        group.MapPost("/", async (Avaliacao avaliacao, MyMovieAPIContext db) =>
        {
            db.Avaliacao.Add(avaliacao);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Avaliacao/{avaliacao.Id}",avaliacao);
        })
        .WithName("CreateAvaliacao")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, MyMovieAPIContext db) =>
        {
            var affected = await db.Avaliacao
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAvaliacao")
        .WithOpenApi();

        group.MapGet("/maior-nota",
            async Task<Results<Ok<Avaliacao>, NotFound>> (MyMovieAPIContext db) =>
            {
                var avaliacao = await db.Avaliacao
                    .AsNoTracking()
                    .OrderByDescending(a => a.Nota)
                    .FirstOrDefaultAsync(); // Pega o primeiro após ordenar

                if (avaliacao == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(avaliacao);
            })
        .WithName("GetAvaliacaoMaiorNota")
        .WithOpenApi();

        group.MapGet("/menor-nota",
            async Task<Results<Ok<Avaliacao>, NotFound>> (MyMovieAPIContext db) =>
            {
                var avaliacao = await db.Avaliacao
                    .AsNoTracking()
                    .OrderBy(a => a.Nota) // Ordenação crescente
                    .FirstOrDefaultAsync();

                if (avaliacao == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(avaliacao);
            })
        .WithName("GetAvaliacaoMenorNota")
        .WithOpenApi();

        group.MapGet("/mais-popular",
            async Task<Results<Ok<Avaliacao>, NotFound>> (MyMovieAPIContext db) =>
            {
                var avaliacao = await db.Avaliacao
                    .AsNoTracking()
                    .OrderByDescending(a => a.Popularidade)
                    .FirstOrDefaultAsync();

                if (avaliacao == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(avaliacao);
            })
        .WithName("GetAvaliacaoMaisPopular")
        .WithOpenApi();

        group.MapGet("/menos-popular",
            async Task<Results<Ok<Avaliacao>, NotFound>> (MyMovieAPIContext db) =>
            {
                var avaliacao = await db.Avaliacao
                    .AsNoTracking()
                    .OrderBy(a => a.Popularidade) // Ordenação crescente
                    .FirstOrDefaultAsync();

                if (avaliacao == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(avaliacao);
            })
        .WithName("GetAvaliacaoMenosPopular")
        .WithOpenApi();

        group.MapGet("/mais-votado",
            async Task<Results<Ok<Avaliacao>, NotFound>> (MyMovieAPIContext db) =>
            {
                var avaliacao = await db.Avaliacao
                    .AsNoTracking()
                    .OrderByDescending(a => a.QtdeVotos)
                    .FirstOrDefaultAsync();

                if (avaliacao == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(avaliacao);
            })
        .WithName("GetAvaliacaoMaisVotado")
        .WithOpenApi();

        group.MapGet("/menos-votado",
            async Task<Results<Ok<Avaliacao>, NotFound>> (MyMovieAPIContext db) =>
            {
                var avaliacao = await db.Avaliacao
                    .AsNoTracking()
                    .OrderBy(a => a.QtdeVotos) // Ordenação crescente
                    .FirstOrDefaultAsync();

                if (avaliacao == null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(avaliacao);
            })
        .WithName("GetAvaliacaoMenosVotado")
        .WithOpenApi();

        group.MapPost("/carga-lote",
            async Task<Results<Created<List<Avaliacao>>, BadRequest>> (List<Avaliacao> avaliacoes, MyMovieAPIContext db) =>
            {
                if (avaliacoes == null || !avaliacoes.Any())
                {
                    return TypedResults.BadRequest();
                }

                db.Avaliacao.AddRange(avaliacoes);

                await db.SaveChangesAsync();

                return TypedResults.Created("/api/Avaliacao", avaliacoes);
            })
        .WithName("CargaLoteAvaliacoes")
        .WithOpenApi()
        .WithSummary("Avaliar vários filmes em lote");
    }
}
