using MediatR;
using MyProject.Application.DTOs;

namespace MyProject.Application.Commands;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    bool IsActive
) : IRequest<ProductDto>;
