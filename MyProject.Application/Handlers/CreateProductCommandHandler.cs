using MediatR;
using MyProject.Application.Commands;
using MyProject.Application.DTOs;
using MyProject.Application.Services;

namespace MyProject.Application.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductService _productService;

    public CreateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        return await _productService.CreateProductAsync(
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.IsActive
        );
    }
}
