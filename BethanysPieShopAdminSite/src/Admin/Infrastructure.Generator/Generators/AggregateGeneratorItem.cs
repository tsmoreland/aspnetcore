using System.Text;

namespace BethanysPieShop.Admin.Infrastructure.Generator.Generators;

internal sealed record class AggregateGeneratorItem(string Namespace, string ClassName, IEnumerable<GeneratorItem> Generators)
    : GeneratorItem(Namespace, ClassName)
{
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
        StringBuilder builder = new();
        foreach (var generator in Generators)
        {
            builder.AppendLine(generator.GenerateClassContent());
        }
        return builder.ToString();
    }
}
