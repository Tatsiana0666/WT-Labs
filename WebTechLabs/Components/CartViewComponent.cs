using Microsoft.AspNetCore.Mvc;
using WebTechLabs.Extensions;
using WebTechLabs.Models;

namespace WebTechLabs.Components
{
    public class CartViewComponent : ViewComponent
    {
        private Cart _cart;
        public CartViewComponent(Cart cart)
        {
            _cart = cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }

    }
}
