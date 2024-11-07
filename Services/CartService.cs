using System.Text.Json;
using Studievereniging.Models;

namespace Studievereniging.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<CartItem> GetCart()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            string? cartJson = session?.GetString(CartSessionKey);
            return cartJson == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        public void SaveCart(List<CartItem> cart)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            string cartJson = JsonSerializer.Serialize(cart);
            session?.SetString(CartSessionKey, cartJson);
        }

        public void AddItem(Product product, int quantity = 1)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.Image
                });
            }

            SaveCart(cart);
        }

        public void RemoveItem(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cart);
            }
        }

        public void ClearCart()
        {
            _httpContextAccessor.HttpContext?.Session.Remove(CartSessionKey);
        }

        public double GetTotal()
        {
            return GetCart().Sum(item => item.Price * item.Quantity);
        }

        public int GetTotalQuantity()
        {
            return GetCart().Sum(item => item.Quantity);
        }
    }
} 