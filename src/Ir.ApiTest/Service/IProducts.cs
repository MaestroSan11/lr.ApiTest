using Ir.IntegrationTest.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Ir.ApiTest.Service;

public interface IProducts
{
  Task<IEnumerable<Product>> GetAll();
  Task<ActionResult<Product>> GetProduct(string id);
  Task<ActionResult<bool>> CreateProduct(Product product);
  Task<ActionResult<bool>> UpdateProduct(string id, JsonPatchDocument<Product> productPatchDocument);
}
