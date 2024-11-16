using BethanysPieShop.Admin.Domain.Models;
using BethanysPieShop.Admin.Domain.Projections;
using BethanysPieShop.Admin.Domain.ValueObjects;

namespace BethanysPieShop.Admin.Domain.Contracts;

public interface ICategoryReadOnlyRepository : IReadOnlyRepository<Category, CategorySummary, CategoriesOrder>;
