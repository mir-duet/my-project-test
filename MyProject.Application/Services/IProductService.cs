using MyProject.Application.DTOs;

namespace MyProject.Application.Services;

public interface IProductService
{
    Task<ProductDto> CreateProductAsync(string name, string? description, decimal price, int stock, bool isActive);
    Task<ProductDto> UpdateProductAsync(Guid id, string name, string? description, decimal price, int stock, bool isActive);
    Task<bool> DeleteProductAsync(Guid id);
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<PaginatedResult<ProductDto>> GetProductsPagedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<ProductDto>> GetActiveProductsAsync();
    Task<bool> IsProductNameUniqueAsync(string name, Guid? excludeId = null);
    Task<ProductDto?> GetProductByNameAsync(string name);
}
