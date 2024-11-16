using Microsoft.CodeAnalysis;

namespace BethanysPieShop.Admin.Infrastructure.Generator.Generators;

internal sealed record class ReadOnlyRepositoryGeneratorItem(string Namespace, string ClassName, string EntityType, string SummaryProjectionType, string OrderEnumType)
    : GeneratorItem(Namespace, ClassName)
{

    /// <summary>
    /// Constructs an instance of <see cref="ReadOnlyRepositoryGeneratorItem"/> if <paramref name="attributeData"/> provides enough arguments;
    /// otherwise <see langword="null"/> is returned.
    /// </summary>
    public static GeneratorItem? Build(string @namespace, string className, AttributeData attributeData)
    {
        const int expectedArgumentCount = 3;
        if (attributeData is not { ConstructorArguments.Length: expectedArgumentCount })
        {
            return null;
        }

        var entityType = (string)attributeData.ConstructorArguments[0].Value!;
        var summaryProjectionType = (string)attributeData.ConstructorArguments[1].Value!;
        var orderEnumType = (string)attributeData.ConstructorArguments[2].Value!;

        return new ReadOnlyRepositoryGeneratorItem(@namespace, className, entityType, summaryProjectionType, orderEnumType);
    }


    /// <summary>
    /// Used to identify the class that needs generation
    /// </summary>
    public static string AttributeName => "BethanysPieShop.Admin.Infrastructure.Persistence.Repositories.ReadOnlyRepositoryAttribute";

    /// <inheritdoc />
    protected override string GenerateSource() 
    {
        return $$"""
            using BethanysPieShop.Admin.Domain.Models;
            using BethanysPieShop.Admin.Domain.ValueObjects;
            using BethanysPieShop.Admin.Infrastructure.Persistence.Extensions;
            using Microsoft.EntityFrameworkCore;

            namespace {{Namespace}};

            partial class {{ClassName}}
            {
            {{GenerateClassContent()}}
            }

            """;
    }

    /// <inheritdoc />
    internal override string GenerateClassContent()
    {
        return $$"""
                public partial IAsyncEnumerable<{{EntityType}}> GetAll({{OrderEnumType}} orderBy, bool descending)
                {
                    return Entities.AsNoTracking()
                        .OrderBy(orderBy, descending)
                        .AsAsyncEnumerable();
                }

                public partial IAsyncEnumerable<{{SummaryProjectionType}}> GetSummaries({{OrderEnumType}} orderBy, bool descending)
                {
                    return GetSummaries(Entities.AsNoTracking().OrderBy(orderBy, descending)).AsAsyncEnumerable();
                }

                public partial Task<{{EntityType}}?> FindById(Guid id, CancellationToken cancellationToken)
                {
                    return GetIncludesForFind(Entities.AsNoTracking()).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
                }

                public async partial ValueTask<Page<{{SummaryProjectionType}}>> GetSummaryPage(PageRequest<{{OrderEnumType}}> request, CancellationToken cancellationToken)
                {
                    int total = await Entities.AsNoTracking().CountAsync(cancellationToken).ConfigureAwait(false);
                    IQueryable<{{EntityType}}> queryable = Entities.AsNoTracking()
                        .Skip(request.GetSkipCount())
                        .Take(request.PageSize);
                    IReadOnlyList<{{SummaryProjectionType}}> items = await GetSummaries(queryable).ToListAsync(cancellationToken).ConfigureAwait(false);
                    return new Page<{{SummaryProjectionType}}>(items, total, request.PageNumber, request.PageSize);
                }
            """;
    }
}
