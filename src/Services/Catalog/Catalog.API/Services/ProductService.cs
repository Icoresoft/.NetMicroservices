using Catalog.API.DbContext;
using Catalog.API.DTO;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Core.Data.Context;

namespace Catalog.API.Services
{

    public class ProductService
    {
        protected readonly IProductRepository productRepository;
        public ProductService(IProductRepository repo)
        {
            productRepository = repo;
        }

        public async Task<ICollection<Entities.Product>> GetAllAsyc()
        {
            return await productRepository.GetAllAsync();
        }
        public async Task<Entities.Product> GetByID(string id)
        {
            return await productRepository.GetAsync(id);
        }
        public async Task<ICollection<Entities.Product>> GetByCategoryAsync(string Category)
        {
            return await productRepository.GetAllAsync(p => p.Category == Category);
        }
        public async Task CreateAsync(DTO.ProductDto p)
        {
            Entities.Product product = new Entities.Product
            {
                Name = p.Name,
                Description = p.Description,
                Summary = p.Summary,
                Price = p.Price,
                ImgFileName=p.ImgFileName,
                Category = p.Category
            };

            await productRepository.CreateAsync(product);
        }
        public async Task<bool> UpdateAsync(DTO.ProductDto p)
        {
            Entities.Product product = new Entities.Product
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Summary = p.Summary,
                Price = p.Price,
                ImgFileName = p.ImgFileName,
                Category = p.Category
            };

            return await productRepository.UpdateAsync(product);
        }
        public async Task<bool> RemoveAsync(string Id)
        {
            return await productRepository.RemoveAsync(Id);
        }
    }
}
