using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studievereniging.Data;
using Studievereniging.Models;
using Studievereniging.Models.ViewModels;

namespace Studievereniging.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationData _context;

        public AdminController(ApplicationData context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var dashboardViewModel = new AdminDashboardViewModel
            {
                Products = await _context.Products.ToListAsync(),
                TotalProducts = await _context.Products.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalUsers = await _context.Users.CountAsync()
            };

            return View(dashboardViewModel);
        }
    }
} 