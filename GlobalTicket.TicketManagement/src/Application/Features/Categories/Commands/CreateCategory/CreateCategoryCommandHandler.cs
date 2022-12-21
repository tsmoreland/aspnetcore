using AutoMapper;
using FluentValidation.Results;
using GlobalTicket.TicketManagement.Application.Contracts.Persistence;
using GlobalTicket.TicketManagement.Domain.Entities;
using MediatR;

namespace GlobalTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryCommandResponse>
{
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }

    /// <inheritdoc />
    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await new CreateCategoryCommandValidator()
            .ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Count > 0)
        {
            return CreateCategoryCommandResponse.CreateFromValidationError(validationResult);
        }

        Category category = new() {Name = request.Name};
        category = await _categoryRepository.AddAsync(category, cancellationToken);
        return new CreateCategoryCommandResponse(_mapper.Map<CreateCategoryDto>(category));
    }
}
