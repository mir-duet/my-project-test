using MediatR;
using MyProject.Application.Commands;
using MyProject.Application.DTOs;
using MyProject.Application.Services;

namespace MyProject.Application.Handlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductService _productService;

    public UpdateProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        return await _productService.UpdateProductAsync(
            request.Id,
            request.Name,
            request.Description,
            request.Price,
            request.Stock,
            request.IsActive
        );
    }
}
