using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Supermarket.API.Domain.Models;

namespace Supermarket.API.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> ListAsync();
        Task AddAsync(Product product);
        Task<Product> FindByIdAsync(int id);
        Task<Product> FindByNameAsync(string name);
        Task<IEnumerable<Product>> FindByCategoryId(int categoryId);
        void Update(Product product);
        void Remove(Product product);
    }
}