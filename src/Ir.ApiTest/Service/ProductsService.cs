using Ir.IntegrationTest.Contracts;
using Ir.IntegrationTest.Entity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Ir.ApiTest.Service;

public class ProductsService : IProducts
{

    Context _context;

    public ProductsService(Context context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAll()
    {
        var products = await _context.Products.ToListAsync();
        List<Product> result = new List<Product>();

        foreach (var prod in products)
        {
            result.Add(MapToProductModel(prod));
        }

        return result;
    }

    public async Task<ActionResult<Product>> GetProduct(string id)
    {
        var existingProduct = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (existingProduct == null)
        {
            return null;
        }

        var finalProduct = MapToProductModel(existingProduct);
        return finalProduct;
    }

    public async Task<ActionResult<bool>> CreateProduct(Product product)
    {
        if (product != null)
        {
            var boolProductExists = await _context.Products.AnyAsync(x => x.Id == product.Id);

            if (!boolProductExists)
            {
                await _context.Products.AddAsync(new Ir.IntegrationTest.Entity.Models.Product()
                {
                    Id = product.Id,
                    Size = product.Size,
                    Colour = product.Colour,
                    Name = product.Name,
                    Price = product.Price,
                    LastUpdated = DateTime.Now,
                    Created = DateTime.Now,
                    Hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(product.Id)))
                });

                await _context.SaveChangesAsync();
                return true;
            }
        }

        return false;
    }

    public async Task<ActionResult<bool>> UpdateProduct(string id, JsonPatchDocument<Product> productPatchDocument)
    {
        var existingProduct = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (existingProduct != null)
        {
            var mappedProduct = MapToProductModel(existingProduct);
            productPatchDocument.ApplyTo(mappedProduct);

            //Update context
            existingProduct.Colour = mappedProduct.Colour;
            existingProduct.Name = mappedProduct.Name;
            existingProduct.Price = mappedProduct.Price;
            existingProduct.Size = mappedProduct.Size;
            existingProduct.Hash = mappedProduct.Hash;
            existingProduct.LastUpdated = DateTime.Now;

            _context.Entry(existingProduct).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    private Product MapToProductModel(Ir.IntegrationTest.Entity.Models.Product product)
    {
        return new Product(
          product.Id,
          product.Size,
          product.Colour,
          product.Name,
          product.Price,
          product.LastUpdated,
          product.Created,
          product.Hash
        );
    }
}