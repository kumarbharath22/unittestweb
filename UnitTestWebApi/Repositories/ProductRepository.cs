
using System.Collections.Generic;
using System.Linq;
using UnitTestWebApi.Data;
using UnitTestWebApi.Models;

namespace UnitTestWebApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private StoreContext _database;

        public ProductRepository(StoreContext database)
        {
            _database = database;
        }

        public List<Product> getAllProducts()
        {
            return _database.products.ToList();
        }

        public Product getProduct(int id)
        {
            return _database.products.Where(product => product.ID == id).SingleOrDefault();
        }


        public void updateProduct(Product product)
        {
            _database.products.Update(product);
            _database.SaveChanges();
        }

        public void deleteProduct(int id)
        {
            Product product = _database.products.Where(product => product.ID == id).SingleOrDefault();
            _database.products.Remove(product);
            _database.SaveChanges();
        }

        public void createProduct(Product product)
        {
            _database.products.Add(product);
            _database.SaveChanges();
        }
    }
}
