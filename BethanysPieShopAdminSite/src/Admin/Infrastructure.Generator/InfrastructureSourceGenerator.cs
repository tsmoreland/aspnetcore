using System.Collections.Immutable;
using System.Collections.ObjectModel;
using BethanysPieShop.Admin.Infrastructure.Generator.Generators;
using BethanysPieShop.Admin.Infrastructure.Generator.PostInitializationItems;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BethanysPieShop.Admin.Infrastructure.Generator;

public sealed class InfrastructureSourceGenerator : IIncrementalGenerator
{
    private delegate GeneratorItem? GeneratorFactory(string @namespace, string className, AttributeData attributeData);

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<GeneratorItem> classes = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (node, _) => IsSyntaxTargetWithAttribute(node),
                transform: static (ctx, _) => GetSemanticTarget(ctx)!) 
            .Where(static target => target is not null);
        context.RegisterSourceOutput(classes, static (ctx, source) =>  Execute(ctx, source));
        context.RegisterPostInitializationOutput(static ctx => PostInitializationOutput(ctx));
    }

    private static bool IsSyntaxTargetWithAttribute(SyntaxNode node)
    {
        return node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };
    }
    private static GeneratorItem? GetSemanticTarget(GeneratorSyntaxContext context)
    {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;
        ISymbol? classSymbol = context.SemanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        if (classSymbol is null)
        {
            return null;
        }

        string @namespace = classSymbol.ContainingNamespace.ToDisplayString();
        string @class = classSymbol.Name;

        IReadOnlyDictionary<string, GeneratorFactory> factoryByAttributeNames = GetGeneratorsWithFactories();

        ReadOnlyCollection<INamedTypeSymbol> supportedAttributes = factoryByAttributeNames.Keys
            .Select(context.SemanticModel.Compilation.GetTypeByMetadataName)
            .Where(static symbol => symbol is not null)
            .Cast<INamedTypeSymbol>()
            .ToList()
            .AsReadOnly();

        ImmutableArray<AttributeData> attributes = classSymbol.GetAttributes();
        return attributes
            .Where(attribute => supportedAttributes.Contains(attribute.AttributeClass, SymbolEqualityComparer.Default))
            .Select(attributeData =>
            { 
                string attributeName = attributeData.AttributeClass?.ToDisplayString() ?? string.Empty;
                return factoryByAttributeNames.TryGetValue(attributeName, out GeneratorFactory factory)
                    ? factory.Invoke(@namespace, @class, attributeData)
                    : null;
            })
            .FirstOrDefault();
    }

    private static void Execute(SourceProductionContext context, GeneratorItem? testItem)
    {
        testItem?.AddSource(context);
    }

    private static void PostInitializationOutput(IncrementalGeneratorPostInitializationContext context)
    {
        foreach (IPostInitializationItem item in GetPostInitializationItems())
        {
            context.AddSource(item.FileName, item.Source);
        }
    }

    private static IReadOnlyDictionary<string, GeneratorFactory> GetGeneratorsWithFactories()
    {
        return new Dictionary<string, GeneratorFactory>
        {
            [ReadOnlyRepositoryGenerator.AttributeName] = ReadOnlyRepositoryGenerator.Build,
        };
    }

    private static IEnumerable<IPostInitializationItem> GetPostInitializationItems()
    {
        yield return new ReadOnlyRepositoryAttributePostInitializationItem();
        yield return new WritableRepositoryAttributePostInitializationItem();
    }
}
