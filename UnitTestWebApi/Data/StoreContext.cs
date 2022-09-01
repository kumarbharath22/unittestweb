using Microsoft.EntityFrameworkCore;
using UnitTestWebApi.Models;

namespace UnitTestWebApi.Data
{
    //boiler plate code to setup the code-first approach to entity framework (interacting with the db)
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options){ }

        //setup the table that will hold all of our products for the store
        public DbSet<Product> products { get; set; }
    }
}
