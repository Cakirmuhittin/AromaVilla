﻿using System.Security.Claims;
using Web.Interfaces;

namespace Web.Services
{
    public class BasketViewModelService : IBasketViewModelService
    {
        private readonly IBasketService _basketService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderService _orderService;

        private HttpContext? HttpContext => _httpContextAccessor.HttpContext;
        private string? USerId => HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        private string? AnonId => HttpContext?.Request.Cookies[Constants.BASKET_COOKIENAME];
        private string BuyerId => USerId ?? AnonId ?? CreateAnonymousId();

        private string _createdAnonId = null!;
        private string CreateAnonymousId()
        {
           if(_createdAnonId ==null)
            {
                _createdAnonId = Guid.NewGuid().ToString();
                HttpContext?.Response.Cookies.Append(Constants.BASKET_COOKIENAME, _createdAnonId, new CookieOptions()
                {
                    Expires = DateTime.Now.AddDays(14),
                    IsEssential = true
                });
            }
           return _createdAnonId;
        }

        public BasketViewModelService(IBasketService basketService,IHttpContextAccessor httpContextAccessor,IOrderService orderService) 
        {
            _basketService = basketService;
            _httpContextAccessor = httpContextAccessor;
            _orderService = orderService;
        }
        public async Task<BasketViewModel> AddItemToBasketAsync(int productId, int quantity)
        {
            var basket = await _basketService.AddItemToBasketAsync(BuyerId,productId,quantity);
           return basket.ToBasketViewModel();
        }

        public async Task<BasketViewModel> GetBasketViewModelAsync()
        {
            var basket = await _basketService.GetOrCreateBasketAsync(BuyerId);
            return basket.ToBasketViewModel();
        }

        public async  Task EmptyBasketAsync()
        {
            await _basketService.EmptyBasketAsync(BuyerId);
        }

        public async Task DeleteBasketItemAsync(int productId)
        {
            await _basketService.DeleteBasketItemAsync(BuyerId, productId);
        }

        public async Task UpdateBasketAsync(Dictionary<int, int> quantities)
        {
            await _basketService.SetQuantities(BuyerId, quantities);
        }

        public async Task TransferBasketAsync()
        {
            if (AnonId == null || USerId == null) return;
            await _basketService.TransferBasketAsync(AnonId, USerId);
            HttpContext?.Response.Cookies.Delete(Constants.BASKET_COOKIENAME);
        }

        public async Task CheckoutAsync(string street, string city, string? state, string country, string zipCode)
        {
            var shippingAddress = new Address(street, city, state, country, zipCode);
            await _orderService.CreateOrderAsync(BuyerId, shippingAddress);
            await _basketService.EmptyBasketAsync(BuyerId);
        }

       
    }
}
