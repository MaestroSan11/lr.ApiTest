using Ir.ApiTest.Service;
using Ir.IntegrationTest.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Ir.FakeMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
  private readonly IProducts _productsService;

  public ProductsController(IProducts productsService)
  {
    _productsService = productsService;
  }

  [HttpGet]
  public async Task<IEnumerable<Product>> GetProducts()
  {
    return (IEnumerable<Product>)await _productsService.GetAll();
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Product>> GetProduct([FromRoute] string id)
  {
    if (string.IsNullOrEmpty(id))
    {
      return BadRequest();
    }

    var result = await _productsService.GetProduct(id);

    //If not a valid product id.
    if (result == null)
    {
      return BadRequest();
    }

    return result;
  }

  [HttpPost()]
  public async Task<ActionResult<bool>> CreateProduct([FromBody] Product product)
  {
    if(product == null || string.IsNullOrEmpty(product.Id))
    {
      return BadRequest();
    }

    var productCreated = await _productsService.CreateProduct(product);

    //if the product already exists.
    if(!productCreated.Value)
    {
      return UnprocessableEntity();
    }

    return Ok();
  }

  [HttpPatch("{id}")]
  public async Task<ActionResult<bool>> UpdateProduct(string id, [FromBody] JsonPatchDocument<Product> productPatchDocument)
  {
    if (string.IsNullOrEmpty(id) || productPatchDocument == null)
    {
      return BadRequest();
    }

    var productUpdated = await _productsService.UpdateProduct(id, productPatchDocument);

    //If not a valid product id.
    if (!productUpdated.Value)
    {
      return BadRequest();
    }

    return Ok();
  }
}