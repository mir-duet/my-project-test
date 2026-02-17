using MediatR;
using MyProject.Application.DTOs;

namespace MyProject.Application.Queries;

public record GetProductsPagedQuery(int PageNumber, int PageSize) : IRequest<PaginatedResult<ProductDto>>;
