using MediatR;
using MyProject.Application.DTOs;
using MyProject.Application.Queries;
using MyProject.Application.Services;

namespace MyProject.Application.Handlers;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductService _productService;

    public GetProductByIdQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _productService.GetProductByIdAsync(request.Id);
    }
}
