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

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BethanysPieShop.Admin.Domain.Contracts;
using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethanysPieShop.MVC.App.Models.Pies;

public sealed class EditViewModel
{
    public EditViewModel()
    {
    }

    [SetsRequiredMembers]
    public EditViewModel(Pie pie, IEnumerable<CategorySummary>? categories)
        : this(categories)
    {
        ArgumentNullException.ThrowIfNull(pie);
        Id = pie.Id;
        Name = pie.Name;
        ShortDescription = pie.ShortDescription;
        LongDescription = pie.LongDescription;
        AllergyInformation = pie.AllergyInformation;
        Price = pie.Price;
        ImageThumbnailUrl = pie.ImageThumbnailFilename;
        ImageUrl = pie.ImageFilename;
        IsPieOfTheWeek = pie.IsPieOfTheWeek;
        InStock = pie.InStock;
        CategoryId = pie.CategoryId;
        ConcurrencyToken = Convert.ToBase64String(pie.ConcurrencyToken);
    }

    [SetsRequiredMembers]
    public EditViewModel(EditViewModel model,  IEnumerable<CategorySummary>? categories)
        : this(categories)
    {
        ArgumentNullException.ThrowIfNull(model);
        Id = model.Id;
        Name = model.Name;
        ShortDescription = model.ShortDescription;
        LongDescription = model.LongDescription;
        AllergyInformation = model.AllergyInformation;
        Price = model.Price;
        ImageThumbnailUrl = model.ImageThumbnailUrl;
        ImageUrl = model.ImageUrl;
        IsPieOfTheWeek = model.IsPieOfTheWeek;
        InStock = model.InStock;
        CategoryId = model.CategoryId;
        ConcurrencyToken = model.ConcurrencyToken;
    }

    public EditViewModel(IEnumerable<CategorySummary>? categories)
    {
        Categories = categories is null
            ? null
            : new SelectList(categories, "Id", "Name", null);
    }

    [Required]
    public required Guid Id { get; set; }

    [Required(ErrorMessage = "Please enter a name")]
    [Display(Name = "Name")]
    [MaxLength(200, ErrorMessage = "The name should be no longer than 200")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Short Description")]
    [MaxLength(100, ErrorMessage = "The short description should be no longer than 100")]
    public string? ShortDescription { get; set; }

    [Display(Name = "Description")]
    [MaxLength(500, ErrorMessage = "The long description should be no longer than 500")]
    public string? LongDescription { get; set; }

    [Display(Name = "Allergy Information")]
    [MaxLength(1000)]
    public string? AllergyInformation { get; set; }

    [Required(ErrorMessage = "Please enter a valid amount")]
    [Display(Name = "Price")]
    [Range(0.0, double.MaxValue)]
    public decimal Price { get; set; } = decimal.Zero;

    [Display(Name = "Image Thumbnail")]
    public string? ImageThumbnailUrl { get; set; }

    [Display(Name = "Image")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Is Pie of the Week")]
    public bool IsPieOfTheWeek { get; set; }

    [Display(Name = "In Stock")]
    public bool InStock { get; set; }

    public Guid? CategoryId { get; set; }

    [Required]
    public string ConcurrencyToken { get; set; } = string.Empty;

    public IEnumerable<SelectListItem>? Categories { get; init; }

    // TODO: Move this logic to application layer, at least the use of the repositories, then this can be void
    public async ValueTask UpdateModel(IPieRepository pieRepository, ICategoryRepository categoryRepository, CancellationToken cancellationToken)
    {
        Pie pie = await pieRepository.FindById(Id, cancellationToken).ConfigureAwait(false) ?? throw new KeyNotFoundException("Pie not found");
        Category? category = CategoryId is not null
            ? await categoryRepository.FindById(CategoryId.Value, cancellationToken).ConfigureAwait(false) ?? throw new KeyNotFoundException("category not found")
            : null;

        pie.Name = Name;
        pie.Price = Price;
        pie.ShortDescription = ShortDescription;
        pie.LongDescription = LongDescription;
        pie.AllergyInformation = AllergyInformation;
        pie.Category = category;
        pie.ImageThumbnailFilename = ImageThumbnailUrl;
        pie.ImageFilename = ImageUrl;
        pie.IsPieOfTheWeek = IsPieOfTheWeek;
        pie.InStock = InStock;
        pieRepository.SetOriginalConcurrencyToken(pie, Convert.FromBase64String(ConcurrencyToken));
    }
}
