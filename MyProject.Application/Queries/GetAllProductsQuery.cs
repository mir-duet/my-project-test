using MediatR;
using MyProject.Application.DTOs;

namespace MyProject.Application.Queries;

public record GetAllProductsQuery() : IRequest<IEnumerable<ProductDto>>;
