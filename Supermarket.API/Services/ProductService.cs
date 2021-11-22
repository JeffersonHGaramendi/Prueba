using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermarket.API.Domain.Models;
using Supermarket.API.Domain.Repositories;
using Supermarket.API.Domain.Services;
using Supermarket.API.Domain.Services.Communication;
using Supermarket.API.Persistence.Repositories;

namespace Supermarket.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await _productRepository.ListAsync();
        }

        public async Task<IEnumerable<Product>> ListByCategoryIdAsync(int categoryId)
        {
            return await _productRepository.FindByCategoryId(categoryId);
        }

        public async Task<ProductResponse> SaveAsync(Product product)
        {
            // VALIDATE CATEGORYID
            
            var existingCategory = _categoryRepository.FindByIdAsync(product.CategoryId);

            if (existingCategory == null)
                return new ProductResponse("Invalid Category.");
            
            // VALIDATE NAME

            var existingProductWithName = await _productRepository.FindByNameAsync(product.Name);

            if (existingProductWithName != null)
                return new ProductResponse("Product Name already exists.");

            try
            {
                await _productRepository.AddAsync(product);
                await _unitOfWork.CompleteAsync();

                return new ProductResponse(product);
            }
            catch (Exception e)
            {
                return new ProductResponse($"An error occurred while saving the product: {e.Message}");
            }
        }

        public async Task<ProductResponse> UpdateAsync(int id, Product product)
        {
            // VALIDATE PRODUCTID
            
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct == null)
                return new ProductResponse("Product not found.");
            
            // VALIDATE CATEGORYID
            
            var existingCategory = _categoryRepository.FindByIdAsync(product.CategoryId);

            if (existingCategory == null)
                return new ProductResponse("Invalid Category.");
            
            // VALIDATE NAME
            
            var existingProductWithName = await _productRepository.FindByNameAsync(product.Name);

            if (existingProductWithName != null && existingProductWithName.Id != existingProduct.Id)
                return new ProductResponse( "Product Name already exists.");

            existingProduct.Name = product.Name;
            existingProduct.UnitOfMeasurement = product.UnitOfMeasurement;
            existingProduct.QuantityInPackage = product.QuantityInPackage;
            existingProduct.CategoryId = product.CategoryId;

            try
            {
                _productRepository.Update(existingProduct);
                await _unitOfWork.CompleteAsync();
                
                return new ProductResponse(existingProduct);
            }
            catch (Exception e)
            {
                return new ProductResponse($"An error occurred while updating the product: {e.Message}");

            }
        }

        public async Task<ProductResponse> DeleteAsync(int id)
        {
            // VALIDATE PRODUCTID
            
            var existingProduct = await _productRepository.FindByIdAsync(id);

            if (existingProduct == null)
                return new ProductResponse("Product not found.");

            try
            {
                _productRepository.Remove(existingProduct);
                await _unitOfWork.CompleteAsync();
                
                return new ProductResponse(existingProduct);
            }
            catch (Exception e)
            {
                return new ProductResponse($"An error occurred while deleting the product: {e.Message}");

            }
        }
    }
}