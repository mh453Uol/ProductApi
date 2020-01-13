using System;
using System.Collections.Generic;
using RefactorThis.Core.Models;

namespace RefactorThis.Peristence.Repositories.SQLite
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        List<Product> GetProductsByName(string name);
        Product GetProductById(Guid id);
        bool ProductExists(Guid id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void RemoveProduct(Guid id);

        List<ProductOption> GetAllProductOptions(Guid productId);
        ProductOption GetProductOptionById(Guid productId, Guid optionId);
        void AddProductOption(ProductOption productOption);
        void UpdateProductOption(ProductOption productOption);
        void RemoveProductOption(Guid id);
        void RemoveAllProductOptions(Guid productId);
    }
}