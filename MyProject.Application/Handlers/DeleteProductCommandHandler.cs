using MediatR;
using MyProject.Application.Commands;
using MyProject.Application.Services;

namespace MyProject.Application.Handlers;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductService _productService;

    public DeleteProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        return await _productService.DeleteProductAsync(request.Id);
    }
}
