﻿// <auto-generated />
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable enable

namespace GloboTicket.Shop.Catalog.Infrastructure.Persistence.CompiledModels
{
    [DbContext(typeof(EventCatalogDbContext))]
    public partial class EventCatalogDbContextModel : RuntimeModel
    {
        static EventCatalogDbContextModel()
        {
            var model = new EventCatalogDbContextModel();
            model.Initialize();
            model.Customize();
            _instance = model;
        }

        private static EventCatalogDbContextModel _instance;
        public static IModel Instance => _instance;

        partial void Initialize();

        partial void Customize();
    }
}
