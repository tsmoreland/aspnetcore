//
// Copyright © 2022 Terry Moreland
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

using AutoMapper;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence.Specifications;
using GlobalTicket.TicketManagement.Application.Features.Categories.Specifications;
using GlobalTicket.TicketManagement.Domain.Common;
using GlobalTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesPageWithEvents;

public sealed class GetCategoriesPageWithEventsQueryHandler : IRequestHandler<GetCategoriesPageWithEventsQuery, Page<CategoryWithEventsViewModel>>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IQueryBuilderFactory _querySpecificationFactory;

    public GetCategoriesPageWithEventsQueryHandler(IMapper mapper,
        ICategoryRepository categoryRepository,
        IQueryBuilderFactory querySpecificationFactory)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _querySpecificationFactory = querySpecificationFactory ?? throw new ArgumentNullException(nameof(querySpecificationFactory));
    }

    /// <inheritdoc />
    public async Task<Page<CategoryWithEventsViewModel>> Handle(GetCategoriesPageWithEventsQuery request, CancellationToken cancellationToken)
    {
        IQueryBuilder<Category> queryBuilder = _querySpecificationFactory.Build<Category>()
            .WithPaging(request.PageRequest)
            .WithOrderBy(new OrderByNameSpecification());

        if (request.IncludeHistory)
        {
            queryBuilder.WithInclusion(new IncludeEventsSpecification());
        }

        return (await _categoryRepository.GetPage(queryBuilder.Query(), cancellationToken))
            .Map<Category, CategoryWithEventsViewModel>(_mapper);

    }

}
