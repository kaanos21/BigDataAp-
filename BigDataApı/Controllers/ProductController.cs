using BigDataApi.Repositories.Abstract;
using BigDataApı.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _productRepository.AddAsync(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productRepository.DeleteAsync(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.ProductId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductDescription = product.ProductDescription;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.CountryOfOrigin = product.CountryOfOrigin;
            existingProduct.ProductImageUrl = product.ProductImageUrl;

            await _productRepository.UpdateAsync(existingProduct);
            return Ok();
        }

        [HttpGet("ProductListWithPaging")]
        public async Task<IActionResult> ProductListWithPaging(int page, int pageSize)
        {
            var products = await _productRepository.ProductListWithPaging(page, pageSize);
            return Ok(products);
        }
        [HttpGet("ProductListWithCategoryAndPaging")]
        public async Task<IActionResult> ProductListWithCategoryAndPaging(int page, int pageSize)
        {
            var products = await _productRepository.ProductListWithCategoryAndPaging(page, pageSize);
            return Ok(products);
        }
    }
}