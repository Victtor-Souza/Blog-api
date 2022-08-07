using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController:ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (System.Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("RFGT55 - Erro interno do servidor"));
            }
           
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] BlogDataContext context, 
            [FromRoute] int id)
        {
            try
            {
                var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

                if (category is null)
                    return StatusCode(404, new ResultViewModel<Category>("Categoria não encontrada."));

            return Ok(new ResultViewModel<Category>(category));
            }
            catch (System.Exception)
           {
                return StatusCode(500, new ResultViewModel<Category>("EX2DB - Erro interno do servidor"));
           }
            
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromServices] BlogDataContext context, 
            [FromBody] EditorCategoryViewModel model)
        {
           try
           {
                if(!ModelState.IsValid)
                    return StatusCode(400, new ResultViewModel<Category>(ModelState.GetErrors()));

                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
           }
           catch(DbUpdateException ex)
           {
                return StatusCode(500, new ResultViewModel<Category>("EX4DF - Não foi possível incluir a categoria!"));
           }
           catch (System.Exception)
           {
                return StatusCode(500, new ResultViewModel<Category>("EX6DE - Erro interno do servidor"));
           }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] BlogDataContext context, 
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                    return StatusCode(400, new ResultViewModel<Category>(ModelState.GetErrors()));

                var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

                if (category is null)
                    return NotFound(new ResultViewModel<Category>("Categoria não entrada"));

                category.Slug = model.Slug;
                category.Name = model.Name;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));    
            }
            catch(DbUpdateException ex)
           {
                return StatusCode(500, new ResultViewModel<Category>("EX1DG - Não foi possível alterar a categoria!"));
           }
           catch (System.Exception)
           {
                return StatusCode(500, new ResultViewModel<Category>("EX5D6 - Erro interno do servidor"));
           }
            
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] BlogDataContext context, 
            [FromRoute] int id
            )
        {
            try
            {
                var category = await context
                .Categories
                .FirstOrDefaultAsync(x => x.Id == id);

                if (category is null)
                    return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch(DbUpdateException ex)
           {
                return StatusCode(500, new ResultViewModel<Category>("ET5HY - Não foi possível remover a categoria!"));
           }
           catch (System.Exception)
           {
                return StatusCode(500, new ResultViewModel<Category>("ET7HZ -Erro interno do servidor"));
           }
        }
    }
}