using Supermarket.API.Domain.Models;

namespace Supermarket.API.Domain.Services.Communication
{
    public class ProductResponse : BaseResponse<Product>
    {
        public ProductResponse(string message) : base(message)
        {
        }

        public ProductResponse(Product resource) : base(resource)
        {
        }
    }
}