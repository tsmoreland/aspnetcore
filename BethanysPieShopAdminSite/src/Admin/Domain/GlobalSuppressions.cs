// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S2365:Properties should not make collection or array copies", Justification = "Copying collection to prevent threading issues during iteration", Scope = "member", Target = "~P:BethanysPieShop.Admin.Domain.Models.Category.Pies")]
[assembly: SuppressMessage("Critical Code Smell", "S2365:Properties should not make collection or array copies", Justification = "Copying collection to prevent threading issues during iteration", Scope = "member", Target = "~P:BethanysPieShop.Admin.Domain.Models.Pie.Ingredients")]
[assembly: SuppressMessage("Critical Code Smell", "S2365:Properties should not make collection or array copies", Justification = "Copying collection to prevent threading issues during iteration", Scope = "member", Target = "~P:BethanysPieShop.Admin.Domain.Models.Order.OrderDetails")]
