﻿//
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
        foreach (GeneratorItem generator in Generators)
        {
            builder.AppendLine(generator.GenerateClassContent());
        }
        return builder.ToString();
    }
}
