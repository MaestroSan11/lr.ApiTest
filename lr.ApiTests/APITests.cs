using Ir.FakeMarketplace.Controllers;
using Ir.ApiTest.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ir.IntegrationTest.Entity;

namespace lr.ApiTests;

public class APITests
{

  ProductsController _controller;
  IProducts _service;
  private readonly Context _context;

  public APITests()
  {
    var builder = new DbContextOptionsBuilder<Context>();
    builder.UseInMemoryDatabase(databaseName: "testDb");

    var dbContextOptions = builder.Options;
    _context = new Context(dbContextOptions);

    _service = new ProductsService(_context);
    _controller = new ProductsController(_service);

    // Delete existing db before creating a new one
    _context.Database.EnsureDeleted();
    _context.Database.EnsureCreated();
  }

  [Fact]
  public void ProductController_ShouldReturnBadRequest_ForGetProduct()
  {
    var result = _controller.GetProduct(null);
    var okResult = result.Result.Result as StatusCodeResult;

    Assert.NotNull(okResult);
    Assert.Equal(400, okResult.StatusCode);
  }

  [Fact]
  public void ProductController_ShouldReturnBadRequest_ForCreateProduct()
  {
    var result = _controller.CreateProduct(null);
    var okResult = result.Result.Result as StatusCodeResult;

    Assert.NotNull(okResult);
    Assert.Equal(400, okResult.StatusCode);
  }
}