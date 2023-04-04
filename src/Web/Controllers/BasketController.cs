using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class BasketController : Controller
    {
        private readonly IBasketViewModelService _basketViewModelService;
        public BasketController(IBasketViewModelService basketViewModelService)
        {
            _basketViewModelService= basketViewModelService;
        }
        public async Task<IActionResult> AddToBasket(int productId,int quantity=1)
        {
           var vm = await _basketViewModelService.AddItemToBasketAsync(productId, quantity);
            return Json(vm);
        }
    }
}
