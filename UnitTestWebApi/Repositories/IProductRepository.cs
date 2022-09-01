using System.Collections.Generic;
using UnitTestWebApi.Models;

namespace UnitTestWebApi.Repositories
{
    public interface IProductRepository
    {
        List<Product> getAllProducts();

        Product getProduct(int id);

        void updateProduct(Product product);

        void deleteProduct(int id);

        void createProduct(Product product);
    }
}
