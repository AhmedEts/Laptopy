using Laptopy.DTOs.Request;
using Laptopy.DTOs.Response;
using Laptopy.Models;
using Laptopy.Repositories.IRepositories;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Laptopy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository,IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            var categories = _categoryRepository.Get();
            //return Ok(categories.Adapt<IEnumerable<CategotyResponse>>());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetOne([FromRoute] int id)
        {
            var category = _categoryRepository.Get(e => e.Id == id);
            if (category != null)
            {
                return Ok(category.Adapt<IEnumerable<CategotyResponse>>());
            }
            return NotFound();
          
        }
        [HttpPost]
        public IActionResult Create([FromBody] CategotyRequest categotyRequest )
        {
       
            Category category = categotyRequest.Adapt<Category>() ;
            _categoryRepository.Create(category);
            _categoryRepository.Comitt();

            return CreatedAtAction(nameof(GetOne), new {Id=category.Id},categotyRequest );
             
        }
        [HttpPut("{id}")]
        public IActionResult Edit([FromRoute]int id, [FromBody] CategotyResponse categotyResponse )
        {
            var category = _categoryRepository.Get(e => e.Id == id);
            if (category!=null)
            {
                _categoryRepository.Edit(new Category
                {
                    Id = id,
                    Name = categotyResponse.Name
                });
                return NoContent();
            }
            return NotFound();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute]int id)
        {
            var category = _categoryRepository.GetOne(e => e.Id == id);
            if (category!=null)
            {
                _categoryRepository.Delete(category);
                return NoContent();
            }
            return NotFound();
        }
       
    }
}
