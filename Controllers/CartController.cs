using Microsoft.AspNetCore.Mvc;
using Studievereniging.Services;
using Studievereniging.Data;
using Studievereniging.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Studievereniging.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ApplicationData _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(CartService cartService, ApplicationData context, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
                return NotFound();

            _cartService.AddItem(product, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            _cartService.RemoveItem(productId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCart();
            if (!cart.Any())
                return RedirectToAction("Index");

            return View("Confirmation", cart);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(string shippingAddress, string notes)
        {
            var cart = _cartService.GetCart();
            if (!cart.Any())
                return RedirectToAction("Index");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var order = new Order
            {
                DateTime = DateTime.Now,
                CustomerId = currentUser.Id,
                user = currentUser,
                OrderLines = cart.Select(item => new OrderLine
                {
                    ProductId = item.ProductId,
                    Amount = item.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _cartService.ClearCart();

            return RedirectToAction("Details", "Orders", new { id = order.Id });
        }

        public int GetTotalItemsCount()
        {
            return _cartService.GetCart().Count;
        }
    }
} 