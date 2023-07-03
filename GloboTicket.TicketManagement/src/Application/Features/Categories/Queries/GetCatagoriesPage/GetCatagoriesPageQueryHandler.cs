using AutoMapper;
using GloboTicket.TicketManagement.Application.Features.Categories.Specifications;
using GloboTicket.TicketManagement.Domain.Common;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Contracts.Persistence.Specifications;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCatagoriesPage;

public sealed class GetCatagoriesPageQueryHandler : IRequestHandler<GetCategoriesPageQuery, Page<CategoryViewModel>>
{
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Category> _categoryRepository;
    private readonly IQueryBuilderFactory _querySpecificationFactory;

    public GetCatagoriesPageQueryHandler(IMapper mapper, IAsyncRepository<Category> categoryRepository, IQueryBuilderFactory querySpecificationFactory)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _querySpecificationFactory = querySpecificationFactory ?? throw new ArgumentNullException(nameof(querySpecificationFactory));
    }

    /// <inheritdoc />
    public async Task<Page<CategoryViewModel>> Handle(GetCategoriesPageQuery request, CancellationToken cancellationToken)
    {
        IQueryBuilder<Category> queryBuilder = _querySpecificationFactory.Build<Category>()
            .WithPaging(request.PageRequest)
            .WithOrderBy(new OrderByNameSpecification());

        return (await _categoryRepository.GetPage(queryBuilder.Query(), cancellationToken: cancellationToken))
            .Map<Category, CategoryViewModel>(_mapper);

    }
}
