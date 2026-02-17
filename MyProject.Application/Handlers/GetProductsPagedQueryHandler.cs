using MediatR;
using MyProject.Application.DTOs;
using MyProject.Application.Queries;
using MyProject.Application.Services;

namespace MyProject.Application.Handlers;

public class GetProductsPagedQueryHandler : IRequestHandler<GetProductsPagedQuery, PaginatedResult<ProductDto>>
{
    private readonly IProductService _productService;

    public GetProductsPagedQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<PaginatedResult<ProductDto>> Handle(GetProductsPagedQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetProductsPagedAsync(request.PageNumber, request.PageSize);
    }
}
