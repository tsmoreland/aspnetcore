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

using System.Text;
using BethanysPieShop.Admin.Infrastructure.Generator.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace BethanysPieShop.Admin.Infrastructure.Generator;

internal abstract record class GeneratorItem(string Namespace, string ClassName)
{
    /// <summary>
    /// Adds the source generated using <see cref="GenerateSource"/> to <paramref name="context"/>
    /// </summary>
    public void AddSource(SourceProductionContext context)
    {
        string source = GenerateSource();
        context.AddSource(Filename, SourceText.From(source, Encoding.UTF8));
    }

    /// <summary>
    /// source filename
    /// </summary>
    protected virtual string Filename => $"{Namespace}.{ClassName}";

    /// <summary>
    /// Generate source code for the current test item
    /// </summary>
    /// <returns></returns>
    protected abstract string GenerateSource();

    /// <summary>
    /// Optionally generates source code for class body, should be overridden
    /// for any generator which may be used as part of <see cref="AggregateGeneratorItem"/>
    /// </summary>
    /// <returns></returns>
    internal virtual string GenerateClassContent()
    {
        return string.Empty;
    }
}
