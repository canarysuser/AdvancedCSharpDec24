using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public short UnitsInStock { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Discontinued { get; set; }
        public int CategoryId { get; set; }
    }
    internal class ProductDbContext: DbContext
    {
        public ProductDbContext() 
            : base(@"server=(local);database=northwind;integrated security=sspi;trustservercertificate=true")
        {
            
        }
        public DbSet<Product> Products { get; set; }
    }
}
