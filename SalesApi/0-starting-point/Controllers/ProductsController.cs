using AutoMapper;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Infrastructure;

namespace SalesApi.Controllers;

[HttpGet("{id}", Name = "GetProduct")]
public async Task<ActionResult<IEnumerable<ProductDTO>>> GetById([FromRoute] string id)
{
    if (!ProductId.IsValid(id))
    {
        return BadRequest("Id is not valid.");
    }

    var canRead = User.HasClaim(c => c.Type == "urn:permissions:products:read" && c.Value == "true");

    if (!canRead)
    {
        return Forbid();
    }

    var product = await productRepository.GetBy(new ProductId(id));

    if (product == null)
    {
        return NotFound();
    }

    if (!User.HasClaim(claim =>
            claim.Type == "urn:permissions:market" &&
            claim.Value == product.MarketId))
    {
        return NotFound();
    }

    return Ok(mapper.Map<ProductDTO>(product));
}