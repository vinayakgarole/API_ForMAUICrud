using demoapi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDBContext _context;

        public ProductsController(ProductDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/products")]
        public async Task<IEnumerable<Product>> Get()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id < 1)
                return BadRequest();
            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
                return NotFound();
            return Ok(product);

        }

		[HttpPost]
		[Route("api/create/")]
		public async Task<IActionResult> Post(Product product)
		{
			if (product == null)
			{
				return BadRequest("Invalid product data.");
			}

			try
			{
				var newProduct = new Product
				{
					Name = product.Name,
					Description = product.Description,
					Price = product.Price
				};

				_context.Add(newProduct);
				await _context.SaveChangesAsync();

				return CreatedAtAction("Get", new { id = newProduct.Id }, newProduct);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while creating the product.");
			}
		}


		[HttpPut]
        [Route("api/update/")]
        public async Task<IActionResult> Put(Product productData)
        {
            if (productData == null || productData.Id == 0)
                return BadRequest();

            var product = await _context.Products.FindAsync(productData.Id);
            if (product == null)
                return NotFound();
            product.Name = productData.Name;
            product.Description = productData.Description;
            product.Price = productData.Price;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("api/products/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
                return BadRequest();
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
