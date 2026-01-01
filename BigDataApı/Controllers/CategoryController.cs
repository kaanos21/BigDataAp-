using BigDataApi.Repositories.Abstract;
using BigDataApı.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return Ok(categories);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            await _categoryRepository.AddAsync(category);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            await _categoryRepository.DeleteAsync(id);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(category.CategoryId);
            if (existingCategory == null)
            {
                return NotFound();
            }
            existingCategory.CategoryName = category.CategoryName;
            await _categoryRepository.UpdateAsync(existingCategory);
            return Ok();
        }
    }
}