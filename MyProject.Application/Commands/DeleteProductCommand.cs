using MediatR;

namespace MyProject.Application.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<bool>;
