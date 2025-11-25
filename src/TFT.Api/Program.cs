using Microsoft.EntityFrameworkCore;
using TFT.Infrastructure.Data;
using TFT.Core.Models;
using System.Text.Json;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<TftContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add OpenAPI services
builder.Services.AddOpenApi();

// Add CORS for future frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapOpenApi();

// Configure Scalar UI for all environments
app.MapScalarApiReference(options =>
{
    options
        .WithTitle("TFT Data API")
        .WithTheme(ScalarTheme.Purple)
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.UseCors("AllowAll");

// API Endpoints

// Get all champions, optionally filtered by trait or cost
app.MapGet("/api/champions", async (TftContext db, string? trait, int? cost, string? set) =>
{
    var query = db.Champions
        .Include(c => c.ChampionTraits)
        .ThenInclude(ct => ct.Trait)
        .AsQueryable();

    if (!string.IsNullOrEmpty(set))
    {
        query = query.Include(c => c.SetData)
            .Where(c => c.SetData.Name == set || c.SetData.Mutator == set);
    }

    if (!string.IsNullOrEmpty(trait))
    {
        query = query.Where(c => c.ChampionTraits.Any(ct => ct.Trait.Name == trait));
    }

    if (cost.HasValue)
    {
        query = query.Where(c => c.Cost == cost.Value);
    }

    var champions = await query.ToListAsync();

    return Results.Ok(champions.Select(c => new
    {
        c.Id,
        c.Name,
        c.Cost,
        c.Icon,
        Traits = c.ChampionTraits.Select(ct => ct.Trait.Name).ToList(),
        Stats = string.IsNullOrEmpty(c.StatsJson) ? null : JsonSerializer.Deserialize<object>(c.StatsJson),
        Ability = string.IsNullOrEmpty(c.AbilityJson) ? null : JsonSerializer.Deserialize<object>(c.AbilityJson)
    }));
})
.WithName("GetChampions")
.WithSummary("Get all champions")
.WithDescription("Get all champions, optionally filtered by trait, cost, or set")
.WithTags("Champions");

// Get a specific champion by name
app.MapGet("/api/champions/{name}", async (TftContext db, string name) =>
{
    var champion = await db.Champions
        .Include(c => c.ChampionTraits)
        .ThenInclude(ct => ct.Trait)
        .Include(c => c.SetData)
        .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());

    if (champion == null)
        return Results.NotFound();

    return Results.Ok(new
    {
        champion.Id,
        champion.Name,
        champion.Cost,
        champion.Role,
        champion.Icon,
        champion.SquareIcon,
        Set = champion.SetData.Name,
        Traits = champion.ChampionTraits.Select(ct => new
        {
            ct.Trait.Name,
            ct.Trait.Icon,
            ct.Trait.Description
        }).ToList(),
        Stats = string.IsNullOrEmpty(champion.StatsJson) ? null : JsonSerializer.Deserialize<object>(champion.StatsJson),
        Ability = string.IsNullOrEmpty(champion.AbilityJson) ? null : JsonSerializer.Deserialize<object>(champion.AbilityJson)
    });
})
.WithName("GetChampionByName")
.WithSummary("Get champion by name")
.WithDescription("Get detailed information about a specific champion")
.WithTags("Champions");

// Get team composition for a trait
app.MapGet("/api/compositions/{trait}", async (TftContext db, string trait, string? set) =>
{
    var query = db.Champions
        .Include(c => c.ChampionTraits)
        .ThenInclude(ct => ct.Trait)
        .Include(c => c.SetData)
        .Where(c => c.ChampionTraits.Any(ct => ct.Trait.Name == trait));

    if (!string.IsNullOrEmpty(set))
    {
        query = query.Where(c => c.SetData.Name == set || c.SetData.Mutator == set);
    }

    var champions = await query
        .OrderBy(c => c.Cost)
        .ToListAsync();

    // Get all unique traits from these champions
    var allTraitIds = champions
        .SelectMany(c => c.ChampionTraits.Select(ct => ct.TraitId))
        .Distinct();

    var synergies = await db.Traits
        .Where(t => allTraitIds.Contains(t.Id))
        .Select(t => new
        {
            t.Name,
            t.Icon,
            Count = champions.Count(c => c.ChampionTraits.Any(ct => ct.TraitId == t.Id))
        })
        .OrderByDescending(t => t.Count)
        .ToListAsync();

    return Results.Ok(new
    {
        Trait = trait,
        Champions = champions.Select(c => new
        {
            c.Name,
            c.Cost,
            c.Icon,
            Traits = c.ChampionTraits.Select(ct => ct.Trait.Name).ToList()
        }),
        Synergies = synergies
    });
})
.WithName("GetComposition")
.WithSummary("Get team composition")
.WithDescription("Get team composition recommendations for a specific trait")
.WithTags("Compositions");

// Get all traits
app.MapGet("/api/traits", async (TftContext db, string? set) =>
{
    var query = db.Traits.AsQueryable();

    if (!string.IsNullOrEmpty(set))
    {
        query = query.Include(t => t.SetData)
            .Where(t => t.SetData.Name == set || t.SetData.Mutator == set);
    }

    var traits = await query.ToListAsync();

    return Results.Ok(traits.Select(t => new
    {
        t.Id,
        t.Name,
        t.Description,
        t.Icon,
        Effects = string.IsNullOrEmpty(t.EffectsJson) ? null : JsonSerializer.Deserialize<object>(t.EffectsJson)
    }));
})
.WithName("GetTraits")
.WithSummary("Get all traits")
.WithDescription("Get all traits, optionally filtered by set")
.WithTags("Traits");

// Get all items
app.MapGet("/api/items", async (TftContext db) =>
{
    var items = await db.Items.ToListAsync();

    return Results.Ok(items.Select(i => new
    {
        i.Id,
        i.Name,
        i.Description,
        i.Icon,
        i.IsUnique,
        Composition = string.IsNullOrEmpty(i.CompositionJson) ? null : JsonSerializer.Deserialize<object>(i.CompositionJson),
        Effects = string.IsNullOrEmpty(i.EffectsJson) ? null : JsonSerializer.Deserialize<object>(i.EffectsJson)
    }));
})
.WithName("GetItems")
.WithSummary("Get all items")
.WithDescription("Get all craftable items with their compositions and effects")
.WithTags("Items");

// Get augments, optionally filtered by tier or trait
app.MapGet("/api/augments", async (TftContext db, int? tier, string? trait) =>
{
    var query = db.Augments.AsQueryable();

    if (tier.HasValue)
    {
        query = query.Where(a => a.Tier == tier.Value);
    }

    var augments = await query.ToListAsync();

    // Filter by trait in memory (since it's stored as JSON)
    if (!string.IsNullOrEmpty(trait))
    {
        augments = augments.Where(a =>
        {
            if (string.IsNullOrEmpty(a.AssociatedTraitsJson))
                return false;

            var traits = JsonSerializer.Deserialize<List<string>>(a.AssociatedTraitsJson);
            return traits?.Any(t => t.Contains(trait, StringComparison.OrdinalIgnoreCase)) ?? false;
        }).ToList();
    }

    return Results.Ok(augments.Select(a => new
    {
        a.Id,
        a.Name,
        a.Description,
        a.Icon,
        a.Tier,
        a.IsUnique,
        Effects = string.IsNullOrEmpty(a.EffectsJson) ? null : JsonSerializer.Deserialize<object>(a.EffectsJson),
        AssociatedTraits = string.IsNullOrEmpty(a.AssociatedTraitsJson) ? null : JsonSerializer.Deserialize<object>(a.AssociatedTraitsJson)
    }));
})
.WithName("GetAugments")
.WithSummary("Get augments")
.WithDescription("Get augments, optionally filtered by tier (1=Silver, 2=Gold, 3=Prismatic) or trait")
.WithTags("Augments");

// Get all available sets
app.MapGet("/api/sets", async (TftContext db) =>
{
    var sets = await db.SetData
        .Select(s => new
        {
            s.Id,
            s.Name,
            s.Mutator,
            s.SetNumber,
            ChampionCount = s.Champions.Count,
            TraitCount = s.Traits.Count
        })
        .ToListAsync();

    return Results.Ok(sets);
})
.WithName("GetSets")
.WithSummary("Get all sets")
.WithDescription("Get all available TFT sets with champion and trait counts")
.WithTags("Sets");

app.Run();
