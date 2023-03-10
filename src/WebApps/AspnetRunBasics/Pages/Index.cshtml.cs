using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;

        public IndexModel(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }

        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _catalogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            string userName = "coresoft";
            //1:get product details
            var product = await _catalogService.GetCatalog(productId);

            var basket = await _basketService.GetBasket(userName);
            basket.Items.Add(
                    new()
                    {
                        ProductId = productId,
                        Quantity = 1,
                        Price = product.Price,
                        ProductName = product.Name,
                        Color = "black",
                    }
                );

            await _basketService.UpdateBasket(basket);
            return RedirectToPage("Cart");
        }
    }
}
