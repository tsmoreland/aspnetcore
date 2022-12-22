using AutoMapper;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Application.Features.Categories.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Categories.Queries.GetCatagoriesPage;

public sealed class GetCatagoriesPageQueryHandler : IRequestHandler<GetCatagoriesPageQuery, Page<CategoryViewModel>>
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
    public async Task<Page<CategoryViewModel>> Handle(GetCatagoriesPageQuery request, CancellationToken cancellationToken)
    {
        IQueryBuilder<Category> queryBuilder = _querySpecificationFactory.Build<Category>()
            .WithPaging(request.PageRequest)
            .WithOrderBy(new OrderByNameSpecification());

        return (await _categoryRepository.GetPage(queryBuilder.Query(), cancellationToken: cancellationToken))
            .Map<Category, CategoryViewModel>(_mapper);

    }
}
