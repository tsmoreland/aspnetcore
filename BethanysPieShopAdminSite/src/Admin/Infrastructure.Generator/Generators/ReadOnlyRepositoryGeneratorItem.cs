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

        string entityType = (string)attributeData.ConstructorArguments[0].Value!;
        string summaryProjectionType = (string)attributeData.ConstructorArguments[1].Value!;
        string orderEnumType = (string)attributeData.ConstructorArguments[2].Value!;

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
                    int total = await Entities.AsNoTracking().CountAsync(cancellationToken);
                    IQueryable<{{EntityType}}> queryable = Entities.AsNoTracking()
                        .Skip(request.GetSkipCount())
                        .Take(request.PageSize);
                    IReadOnlyList<{{SummaryProjectionType}}> items = await GetSummaries(queryable).ToListAsync(cancellationToken);
                    return new Page<{{SummaryProjectionType}}>(items, total, request.PageNumber, request.PageSize);
                }
            """;
    }
}
