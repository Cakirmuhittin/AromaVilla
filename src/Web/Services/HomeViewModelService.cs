using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Interfaces;

namespace Web.Services
{
    public class HomeViewModelService : IHomeViewModelService
    {
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Brand> _brandRepo;
        private readonly IRepository<Product> _productRepo;

        public HomeViewModelService(IRepository<Category> categoryRepo, IRepository<Brand> brandRepo,IRepository<Product> productRepo)
        {
            _categoryRepo = categoryRepo;
            _brandRepo = brandRepo;
            _productRepo = productRepo;
        }
        public async Task<HomeViewModel> GetHomeViewModelAsync(int? categoryId, int? brandId, int pageId = 1)
        {
            var products = await _productRepo.GetAllAsync();
            var vm = new HomeViewModel()
            {
                Products = products.Select(x => new ProductViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    PictureUri = x.PictureUri,
                    Price = x.Price
                }).ToList(),
                Categories = await GetCategoriesAsync(),
                Brands= await GetBrandsAsync(),
                CategoryId=categoryId,
                BrandId=brandId
            };
            return vm;
        }

        private async Task<List<SelectListItem>> GetBrandsAsync()
        {
            var brands = await _brandRepo.GetAllAsync();
            return brands.Select(x => new SelectListItem()
            {
            Text= x.Name,
            Value=x.Id.ToString()
            }).ToList();
        }

        private async Task<List<SelectListItem>> GetCategoriesAsync()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
        }
    }
}
