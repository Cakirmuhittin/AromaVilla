using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class BasketService : IBasketService
    {
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRepository<BasketItem> _basketItemRepo;
        private readonly IRepository<Product> _productRepo;

        public BasketService(IRepository<Basket> basketRepository,IRepository<BasketItem> basketItemRepo,IRepository<Product> productRepo)
        {
            _basketRepository = basketRepository;
            _basketItemRepo = basketItemRepo;
            _productRepo = productRepo;
        }

        public async Task<Basket> AddItemToBasketAsync(string buyerId, int producId, int quantity)
        {
            var basket = await GetOrCreateBasketAsync(buyerId);
            var basketItem = basket.BasketItems.FirstOrDefault(x=>x.ProductId== producId);

            if(basketItem ==null)
            {
                basketItem = new BasketItem()
                {
                    ProductId = producId,
                    Quantity = quantity,
                    Product = (await _productRepo.GetByIdAsync(producId))!
                };
                basket.BasketItems.Add(basketItem);
            }
            else
            {
                basketItem.Quantity += quantity;
            }
            await _basketRepository.UpdateAsync(basket);
            return basket;
        }

        public async Task DeleteBasketItemAsync(string buyerId, int productId)
        {
            var basket = await GetOrCreateBasketAsync(buyerId);
            var basketItem=basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (basketItem == null) return;

            await _basketItemRepo.DeleteAsync(basketItem);
        }

        public async Task EmptyBasketAsync(string buyerId)
        {
            var basket = await GetOrCreateBasketAsync(buyerId);
            foreach(var item in basket.BasketItems.ToList())
            {
                await _basketItemRepo.DeleteAsync(item);
            }
        }

        public async Task<Basket> GetOrCreateBasketAsync(string buyerId)
        {
            var specBasket = new BasketWithItemsSpecification(buyerId);
            var basket = await _basketRepository.FirstOrDefaultAsync(specBasket);
            if(basket==null)
            {
                basket=new Basket() { BuyerId=buyerId};
                await _basketRepository.AddAsync(basket);
            }
            return basket;

        }

        public async Task<Basket> SetQuantities(string buyerId, Dictionary<int, int> quantities)
        {
            var basket = await GetOrCreateBasketAsync(buyerId);

            foreach(var item in basket.BasketItems)
            {
                if(quantities.ContainsKey(item.ProductId))
                {
                    item.Quantity = quantities[item.ProductId];
                    await _basketItemRepo.UpdateAsync(item);
                }
            }
            return basket;
        }

        public async Task TransferBasketAsync(string sourceBuyerId, string destinationBuyerId)
        {
            var specSourceBasket = new BasketWithItemsSpecification(sourceBuyerId);
            var sourceBasket = await _basketRepository.FirstOrDefaultAsync(specSourceBasket);

            if (sourceBasket == null || sourceBasket.BasketItems.Count == 0) return;

            var destinationBasket = await GetOrCreateBasketAsync(destinationBuyerId);

            foreach(var item in sourceBasket.BasketItems)
            {
                var targetItem = destinationBasket.BasketItems.FirstOrDefault(x=>x.ProductId == item.ProductId);

                if(targetItem == null)
                {
                    destinationBasket.BasketItems.Add(new BasketItem()
                    {
                        ProductId= item.ProductId,
                        Quantity= item.Quantity
                    });
                }
                else
                {
                    targetItem.Quantity += item.Quantity;
                }
                await _basketRepository.UpdateAsync(destinationBasket);
                await _basketRepository.DeleteAsync(sourceBasket);
            }
        }

    }
}
