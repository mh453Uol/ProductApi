using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using RefactorThis.Core.Models;
using RefactorThis.Peristence.Repositories.SQLite;
using RefactorThis.Utilities;

namespace RefactorThis.Persistence.Repositories.SQLite.ADO.NET
{
    public class ProductRepository : IProductRepository
    {
        public void AddProduct(Product product)
        {
            var query = @"INSERT INTO Products (id, name, description, price, deliveryprice)
                values (@Id, @Name, @Description, @Price, @DeliveryPrice)";

            SqliteParameter[] parameters =  {
                new SqliteParameter("@Id", product.Id),
                new SqliteParameter("@Name", product.Name),
                new SqliteParameter("@Description", product.Description),
                new SqliteParameter("@Price", product.Price),
                new SqliteParameter("@DeliveryPrice", product.DeliveryPrice)
            };

            SqliteUtil.Insert(query, parameters);
        }

        public void RemoveProduct(Guid productId)
        {
            var query = $"DELETE FROM Products WHERE id = @productId collate nocase";

            SqliteParameter[] parameters = { new SqliteParameter("@productId", productId) };

            SqliteUtil.Delete(query, parameters);
        }

        public List<Product> GetAllProducts()
        {
            var query = "SELECT * FROM Products";

            var products = SqliteUtil.Query<Product>(toProduct, query);

            return products;
        }

        public bool ProductExists(Guid id)
        {
            var query = "SELECT id FROM Products WHERE id = @Id collate nocase";

            SqliteParameter[] parameters = { new SqliteParameter("@Id", id) };

            var products = SqliteUtil.Query<Product>(
                (SqliteDataReader reader) => { 
                   return new Product() { Id = Guid.Parse(reader.GetValueOrDefault<string>("Id")) };
                }, query, parameters);

            return products.Count > 0;
        }

        public Product GetProductById(Guid id)
        {
            var query = "SELECT * FROM Products WHERE id = @id collate nocase";

            SqliteParameter[] parameters = { new SqliteParameter("@id", id) };

            var products = SqliteUtil.Query<Product>(toProduct, query, parameters);

            if (products.Count == 1)
            {
                return products[0];
            }

            return null;
        }

        public List<Product> GetProductsByName(string name)
        {
            var query = "SELECT * FROM Products WHERE LOWER(name) LIKE @Name";

            SqliteParameter[] parameters = { new SqliteParameter("@Name", $"%{name.ToLower()}%") };

            var products = SqliteUtil.Query<Product>(toProduct, query, parameters);

            return products;
        }

        public void UpdateProduct(Product product)
        {
            var query = @"UPDATE Products SET name = @Name, description = @Description, 
                price = @Price, deliveryprice = @DeliveryPrice 
                where id = @Id collate nocase";

            SqliteParameter[] parameters =  {
                new SqliteParameter("@Id", product.Id),
                new SqliteParameter("@Name", product.Name),
                new SqliteParameter("@Description", product.Description),
                new SqliteParameter("@Price", product.Price),
                new SqliteParameter("@DeliveryPrice", product.DeliveryPrice),
            };

            SqliteUtil.Update(query, parameters);
        }

        public List<ProductOption> GetAllProductOptions(Guid productId)
        {
            var query = "SELECT * FROM ProductOptions WHERE productId = @productId collate nocase";

            SqliteParameter[] parameters = { new SqliteParameter("@productId", productId) };

            var options = SqliteUtil.Query<ProductOption>(toProductOption, query, parameters);

            return options;
        }

        public ProductOption GetProductOptionById(Guid productId, Guid optionId)
        {
            var query = "SELECT * FROM productoptions WHERE productId = @productId collate nocase AND id = @optionId collate nocase";

            SqliteParameter[] parameters = {
                new SqliteParameter("@productId", productId),
                new SqliteParameter("@optionId", optionId),
            };

            var options = SqliteUtil.Query<ProductOption>(toProductOption, query, parameters);

            if (options.Count == 1)
            {
                return options[0];
            }

            return null;
        }

        public void AddProductOption(ProductOption productOption)
        {
            var query = @"INSERT INTO productoptions (id, productid, name, description) 
                values (@Id, @ProductId, @Name, @Description)";

            SqliteParameter[] parameters =  {
                new SqliteParameter("@Id", productOption.Id),
                new SqliteParameter("@ProductId", productOption.ProductId),
                new SqliteParameter("@Name", productOption.Name),
                new SqliteParameter("@Description", productOption.Description),
            };

            SqliteUtil.Insert(query, parameters);
        }

        public void UpdateProductOption(ProductOption productOption)
        {
            var query = @"UPDATE productoptions SET name = @Name, description = @Description 
                where id = @Id collate nocase";

            SqliteParameter[] parameters =  {
                new SqliteParameter("@Name", productOption.ProductId),
                new SqliteParameter("@Description", productOption.Description),
                new SqliteParameter("@Id", productOption.Id)
            };

            SqliteUtil.Update(query, parameters);
        }

        public void RemoveProductOption(Guid id)
        {
            var query = "DELETE FROM ProductOptions WHERE id = @Id collate nocase";

            SqliteParameter[] parameters = { new SqliteParameter("@Id", id) };

            SqliteUtil.Delete(query, parameters);
        }

        public void RemoveAllProductOptions(Guid productId)
        {
            var query = "DELETE FROM ProductOptions WHERE productId = @productId collate nocase";

            SqliteParameter[] parameters = { new SqliteParameter("@productId", productId) };

            SqliteUtil.Delete(query, parameters);
        }

        private Product toProduct(SqliteDataReader reader)
        {
            return new Product()
            {
                Id = Guid.Parse(reader.GetValueOrDefault<string>("Id")),
                Name = reader.GetValueOrDefault<string>("Name"),
                Description = reader.GetValueOrDefault<string>("Description"),
                Price = decimal.Parse(reader.GetValueOrDefault<string>("Price")),
                DeliveryPrice = decimal.Parse(reader.GetValueOrDefault<string>("DeliveryPrice"))
            };
        }

        private ProductOption toProductOption(SqliteDataReader reader)
        {
            return new ProductOption()
            {
                Id = Guid.Parse(reader.GetValueOrDefault<string>("Id")),
                Name = reader.GetValueOrDefault<string>("Name"),
                Description = reader.GetValueOrDefault<string>("Description"),
                ProductId = Guid.Parse(reader.GetValueOrDefault<string>("ProductId"))
            };
        }

    }
}
