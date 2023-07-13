using CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.DataStorePluginInterfaces;

namespace Plugins.DataStore.InMemory
{
    public class ProductInMemoryRepository : IProductRepository
    {
        public List<Product> products { get; set; }

        public ProductInMemoryRepository()
        {
            //Init with default values
            products = new List<Product>()
            {
                new Product{ Id = 1, CategoryId = 1, Name = "White Shirt", Quantity = 100, Price = 450, Details = "details", Description = "Description" },
                new Product{ Id = 2, CategoryId = 1, Name = "Black Shirt", Quantity = 200, Price = 499, Details = "details", Description = "Description"},
                new Product{ Id = 3, CategoryId = 2, Name = "Lee", Quantity = 300, Price = 999, Details = "details", Description = "Description"},
                new Product{ Id = 4, CategoryId = 2, Name = "Black Pants", Quantity = 400, Price = 349, Details = "details", Description = "Description"},
                new Product{ Id = 5, CategoryId = 3, Name = "W Brown", Quantity = 70, Price = 1400, Details = "details", Description = "Description"},
                new Product{ Id = 6, CategoryId = 3, Name = "Nike", Quantity = 160, Price = 3500, Details = "details", Description = "Description"}
            };
        }
        public IEnumerable<Product> GetProducts()
        {
            return products;
        }

        public void AddProduct(Product product)
        {
            if (products.Any(x => x.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase))) return;

            if (products != null && products.Count > 0)
            {
                var maxId = products.Max(x => x.Id);
                product.Id = maxId + 1;
            }
            else
            {
                product.Id = 1;
            }

            products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            var categoryToUpdate = GetProductById(product.Id);
            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = product.Name;
                categoryToUpdate.Price = product.Price;
                categoryToUpdate.Quantity = product.Quantity;
                categoryToUpdate.CategoryId = product.CategoryId;
                categoryToUpdate.Details = product.Details;
                categoryToUpdate.Description = product.Description;
            };
        }

        public Product GetProductById(int productId)
        {
            return products?.FirstOrDefault(p => p.Id == productId);
        }

        public void DeleteProduct(int productId)
        {
            products?.Remove(GetProductById(productId));
        }

        public IEnumerable<Product> GetProductsByCategoryId(int categoryId)
        {
            return products.Where(p => p.CategoryId == categoryId);
        }
    }
}
