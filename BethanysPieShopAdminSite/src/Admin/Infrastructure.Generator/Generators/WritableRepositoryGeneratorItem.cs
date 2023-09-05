//
// Copyright © 2023 Terry Moreland
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

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

        string entityType = (string)attributeData.ConstructorArguments[0].Value!;

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
                    await ValidateAddOrThrow(entity, cancellationToken);
                    Entities.Add(entity);
                }

                public async partial ValueTask Update({{EntityType}} entity, CancellationToken cancellationToken)
                {
                    ArgumentNullException.ThrowIfNull(entity);
                    await ValidateUpdateOrThrow(entity, cancellationToken);
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
                    await GetIncludesForFindTracked(entity, cancellationToken);
                    Delete(entity);
                }

                public async partial ValueTask SaveChanges(CancellationToken cancellationToken)
                {
                    await _dbContext.SaveChangesAsync(cancellationToken);
                }

            """;
    }
}
