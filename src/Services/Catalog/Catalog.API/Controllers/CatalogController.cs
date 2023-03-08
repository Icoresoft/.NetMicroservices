using Catalog.API.DbContext;
using Catalog.API.Repositories;
using Catalog.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        protected readonly ProductService Service;
        public CatalogController(ProductService service)
        {
            Service = service;
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Entities.Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Entities.Product>>> GetAllAsync()
        {
            Thread.Sleep(3000);// for testing Ocelot Gateway cache
            var data = await Service.GetAllAsyc();
            return Ok(data);
        }
        //catalog/Id
        [HttpGet("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Entities.Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Entities.Product>>> GetById(string Id)
        {
            var data = await Service.GetByID(Id);
            if(data is null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpGet("[Action]/{Category}")]
        [ProducesResponseType(typeof(IEnumerable<Entities.Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Entities.Product>>> GetByCategory(string Category)
        {
            var data = await Service.GetByCategoryAsync(Category);
            return Ok(data);
        }

        //[HttpPost("Create")]
        [HttpPost]
        [ProducesResponseType( (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody]DTO.ProductDto Product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           await Service.CreateAsync(Product);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update([FromBody] DTO.ProductDto Product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool result=await Service.UpdateAsync(Product);
           
            return result? Ok():BadRequest("NOT UPDATED");
        }
        //Catalog/Remove/Id
        [HttpDelete("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> Remove(string Id)
        {
            bool result = await Service.RemoveAsync(Id);

            return result ? Ok() : BadRequest("NOT DELETED");
        }
    }
}
