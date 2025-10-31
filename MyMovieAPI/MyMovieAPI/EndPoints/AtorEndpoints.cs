using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using MyMovieAPI.Data;
using MyMovieAPI.Models;
using Microsoft.AspNetCore.Mvc;
namespace MyMovieAPI.EndPoints;

public static class AtorEndpoints
{
    public static void MapAtorEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Ator").WithTags(nameof(Ator));

        group.MapGet("/", async (MyMovieAPIContext db) =>
        {
            return await db.Ator.ToListAsync();
        })
        .WithName("GetAllAtors")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Ator>, NotFound>> (int id, MyMovieAPIContext db) =>
        {
            return await db.Ator.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Ator model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetAtorById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Ator ator, MyMovieAPIContext db) =>
        {
            var affected = await db.Ator
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Nome, ator.Nome)
                    .SetProperty(m => m.DataNascimento, ator.DataNascimento)
                    .SetProperty(m => m.Nacionalidade, ator.Nacionalidade)
                    .SetProperty(m => m.Id, ator.Id)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateAtor")
        .WithOpenApi();

        group.MapPost("/", async (Ator ator, MyMovieAPIContext db) =>
        {
            db.Ator.Add(ator);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Ator/{ator.Id}", ator);
        })
        .WithName("CreateAtor")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, MyMovieAPIContext db) =>
        {
            var affected = await db.Ator
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteAtor")
        .WithOpenApi();

        group.MapGet("/buscar/nome/{nome}",
             async Task<Results<Ok<List<Ator>>, NotFound>> ([FromRoute] string nome, MyMovieAPIContext db) =>
             {
                 var atores = await db.Ator
                    .AsNoTracking()
                    // Busca case-insensitive e parcial (Contains)
                    .Where(a => a.Nome.Contains(nome))
                    .ToListAsync();

                 if (!atores.Any())
                 {
                     return TypedResults.NotFound();
                 }

                 return TypedResults.Ok(atores);
             })
         .WithName("BuscarAtorPorNome")
         .WithOpenApi();

        group.MapGet("/buscar/nacionalidade/{nacionalidade}",
            async Task<Results<Ok<List<Ator>>, NotFound>> ([FromRoute] string nacionalidade, MyMovieAPIContext db) =>
            {
                var atores = await db.Ator
                    .AsNoTracking()
                    // Busca case-insensitive e exata (Equals)
                    .Where(a => a.Nacionalidade.Equals(nacionalidade))
                    .ToListAsync();

                if (!atores.Any())
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(atores);
            })
        .WithName("BuscarAtorPorNacionalidade")
        .WithOpenApi();

        group.MapPost("/carga-lote",
            async Task<Results<Created<List<Ator>>, BadRequest>> (List<Ator> atores, MyMovieAPIContext db) =>
            {
                if (atores == null || !atores.Any())
                {
                    return TypedResults.BadRequest(); // Retorna 400 se a lista estiver vazia
                }

                db.Ator.AddRange(atores);

                await db.SaveChangesAsync();

                return TypedResults.Created("/api/Ator", atores);
            })
        .WithName("CargaLoteAtores")
        .WithOpenApi()
        .WithSummary("Cadastrar vários atores");

    }
}
