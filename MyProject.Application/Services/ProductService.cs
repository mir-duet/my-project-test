using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Application.Exceptions;
using MyProject.Application.Interfaces;
using MyProject.Domain.Entities;

namespace MyProject.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> CreateProductAsync(string name, string? description, decimal price, int stock, bool isActive)
    {
        // Validate price >= 0
        if (price < 0)
        {
            throw new ValidationException("Price cannot be negative.");
        }

        // Check for duplicate product names
        if (await _unitOfWork.Repository<Product>().AnyAsync(p => p.Name == name))
        {
            throw new ValidationException($"Product with name '{name}' already exists.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = price,
            Stock = stock,
            IsActive = isActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Product>().AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, string name, string? description, decimal price, int stock, bool isActive)
    {
        // Check product exists
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product == null)
        {
            throw new NotFoundException("Product", id);
        }

        // Validate price >= 0
        if (price < 0)
        {
            throw new ValidationException("Price cannot be negative.");
        }

        // Validate new name doesn't conflict with other products
        if (await _unitOfWork.Repository<Product>().AnyAsync(p => p.Name == name && p.Id != id))
        {
            throw new ValidationException($"Product with name '{name}' already exists.");
        }

        // Business rule: Cannot deactivate if stock < 10
        if (!isActive && product.IsActive && product.Stock < 10)
        {
            throw new ValidationException("Cannot deactivate product with stock less than 10.");
        }

        product.Name = name;
        product.Description = description;
        product.Price = price;
        product.Stock = stock;
        product.IsActive = isActive;
        product.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Repository<Product>().Update(product);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> DeleteProductAsync(Guid id)
    {
        // Check product exists
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product == null)
        {
            throw new NotFoundException("Product", id);
        }

        // Business rule: Cannot delete if stock > 0
        if (product.Stock > 0)
        {
            throw new ValidationException("Cannot delete product with existing stock.");
        }

        _unitOfWork.Repository<Product>().Delete(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product == null)
        {
            throw new NotFoundException("Product", id);
        }

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Repository<Product>().GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<PaginatedResult<ProductDto>> GetProductsPagedAsync(int pageNumber, int pageSize)
    {
        var products = await _unitOfWork.Repository<Product>().GetPagedAsync(pageNumber, pageSize);
        var totalCount = await _unitOfWork.Repository<Product>().CountAsync();

        return new PaginatedResult<ProductDto>
        {
            Items = _mapper.Map<IEnumerable<ProductDto>>(products),
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
    {
        var products = await _unitOfWork.Repository<Product>().FindAsync(p => p.IsActive);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<bool> IsProductNameUniqueAsync(string name, Guid? excludeId = null)
    {
        return !await _unitOfWork.Repository<Product>().AnyAsync(
            p => p.Name == name && (!excludeId.HasValue || p.Id != excludeId.Value));
    }

    public async Task<ProductDto?> GetProductByNameAsync(string name)
    {
        var product = await _unitOfWork.Repository<Product>().FirstOrDefaultAsync(p => p.Name == name);
        return product != null ? _mapper.Map<ProductDto>(product) : null;
    }
}
