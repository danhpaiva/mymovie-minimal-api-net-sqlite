using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using MyMovieAPI.Data;
using MyMovieAPI.Models;
namespace MyMovieAPI.EndPoints;

public static class FilmeAtorEndpoints
{
    public static void MapFilmeAtorEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/FilmeAtor").WithTags(nameof(FilmeAtor));

        group.MapGet("/", async (MyMovieAPIContext db) =>
        {
            return await db.FilmeAtor.ToListAsync();
        })
        .WithName("GetAllFilmeAtors")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<FilmeAtor>, NotFound>> (int id, MyMovieAPIContext db) =>
        {
            return await db.FilmeAtor.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is FilmeAtor model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetFilmeAtorById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, FilmeAtor filmeAtor, MyMovieAPIContext db) =>
        {
            var affected = await db.FilmeAtor
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.FilmeId, filmeAtor.FilmeId)
                    .SetProperty(m => m.AtorId, filmeAtor.AtorId)
                    .SetProperty(m => m.Id, filmeAtor.Id)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateFilmeAtor")
        .WithOpenApi();

        group.MapPost("/", async (FilmeAtor filmeAtor, MyMovieAPIContext db) =>
        {
            db.FilmeAtor.Add(filmeAtor);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/FilmeAtor/{filmeAtor.Id}",filmeAtor);
        })
        .WithName("CreateFilmeAtor")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, MyMovieAPIContext db) =>
        {
            var affected = await db.FilmeAtor
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteFilmeAtor")
        .WithOpenApi();

        group.MapPost("/carga-lote",
            async Task<Results<Created<List<FilmeAtor>>, BadRequest>> (List<FilmeAtor> ligacoes, MyMovieAPIContext db) =>
            {
                if (ligacoes == null || !ligacoes.Any())
                {
                    return TypedResults.BadRequest(); 
                }

                db.FilmeAtor.AddRange(ligacoes);

                await db.SaveChangesAsync();

                return TypedResults.Created("/api/FilmeAtor", ligacoes);
            })
        .WithName("CargaLoteFilmeAtor")
        .WithOpenApi()
        .WithSummary("Cadastra diversos relacionamentos");
    }
}
