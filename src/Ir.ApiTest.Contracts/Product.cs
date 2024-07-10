using System;

namespace Ir.IntegrationTest.Contracts
{
  public class Product
  {
    public Product(string id, string size, string colour, string name, double price, DateTimeOffset lastUpdated, DateTimeOffset created, string hash)
    {
      Id = id;
      Size = size;
      Colour = colour;
      Name = name;
      Price = price;
      LastUpdated = lastUpdated;
      Created = created;
      Hash = hash;
    }

    public string Id { get; set; }
    public string Size { get; set; }
    public string Colour { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset Created { get; set; }
    public string Hash { get; set; }
  }}