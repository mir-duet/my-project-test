using MediatR;
using MyProject.Application.DTOs;

namespace MyProject.Application.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;
