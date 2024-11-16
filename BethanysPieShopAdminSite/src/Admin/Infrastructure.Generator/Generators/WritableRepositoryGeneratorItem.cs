using Microsoft.CodeAnalysis;

namespace BethanysPieShop.Admin.Infrastructure.Generator.Generators;

internal sealed record class WritableRepositoryGeneratorItem(string Namespace, string ClassName, string EntityType) : GeneratorItem(Namespace, ClassName)
{
    /// <summary>
    /// Constructs an instance of <see cref="ReadOnlyRepositoryGeneratorItem"/> if <paramref name="attributeData"/> provides enough arguments;
    /// otherwise <see langword="null"/> is returned.
    /// </summary>
    public static GeneratorItem? Build(string @namespace, string className, AttributeData attributeData)
    {
        const int expectedArgumentCount = 1;
        if (attributeData is not { ConstructorArguments.Length: expectedArgumentCount })
        {
            return null;
        }

        var entityType = (string)attributeData.ConstructorArguments[0].Value!;

        return new WritableRepositoryGeneratorItem(@namespace, className, entityType);
    }

    /// <summary>
    /// Used to identify the class that needs generation
    /// </summary>
    public static string AttributeName => "BethanysPieShop.Admin.Infrastructure.Persistence.Repositories.WritableRepositoryAttribute";

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
                public async partial ValueTask Add({{EntityType}} entity, CancellationToken cancellationToken)
                {
                    ArgumentNullException.ThrowIfNull(entity);
                    await ValidateAddOrThrow(entity, cancellationToken).ConfigureAwait(false);
                    Entities.Add(entity);
                }

                public async partial ValueTask Update({{EntityType}} entity, CancellationToken cancellationToken)
                {
                    ArgumentNullException.ThrowIfNull(entity);
                    await ValidateUpdateOrThrow(entity, cancellationToken).ConfigureAwait(false);
                    Entities.Update(entity);
                }

                public partial void Delete({{EntityType}} entity)
                {
                    if (Entities.Contains(entity))
                    {
                        Entities.Remove(entity);
                    }
                }

                public async partial ValueTask Delete(Guid id, CancellationToken cancellationToken)
                {
                    (bool allowed, string reason) = await AllowsDelete(id, cancellationToken);
                    if (!allowed)
                    {
                        throw new ArgumentException(reason, nameof(id));
                    }

                    {{EntityType}} entity = await Entities.FindAsync(new object[] { id }, cancellationToken) ??
                        throw new ArgumentException($"Entity matching {id} not found", nameof(id));
                    await GetIncludesForFindTracked(entity, cancellationToken).ConfigureAwait(false);
                    Delete(entity);
                }

                public async partial ValueTask SaveChanges(CancellationToken cancellationToken)
                {
                    await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                }

            """;
    }
}
