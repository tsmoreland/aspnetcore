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
using BethanysPieShop.Admin.Domain.Projections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethanysPieShop.MVC.App.Models.Pies;

public sealed class AddViewModel
{
    public AddViewModel()
    {
    }
    public AddViewModel(IEnumerable<CategorySummary>? categories)
    {
        Categories = categories is null
            ? null
            : new SelectList(categories, "Id", "Name", null);
    }

    [Required(ErrorMessage = "Please enter a name")]
    [Display(Name = "Name")]
    [MaxLength(200, ErrorMessage = "The name shouild be no longer than 200")]
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

    [Display(Name = "Image Thumbndail")]
    public string? ImageThumbnailUrl { get; set; }

    [Display(Name = "Image")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Is Pie of the Week")]
    public bool IsPieOfTheWeek { get; set; }

    [Display(Name = "In Stock")]
    public bool InStock { get; set; }

    public Guid CategoryId { get; set; }

    public IEnumerable<SelectListItem>? Categories { get; init; }
}
