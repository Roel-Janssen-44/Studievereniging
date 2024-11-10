using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studievereniging.Data;
using Studievereniging.Models;

namespace Studievereniging.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersApiController : ControllerBase
    {
        private readonly ApplicationData _context;

        public OrdersApiController(ApplicationData context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all orders with their order lines and products
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.user)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .Select(o => new
                {
                    o.Id,
                    o.DateTime,
                    CustomerName = o.user.UserName,
                    CustomerEmail = o.user.Email,
                    TotalAmount = o.OrderLines.Sum(ol => ol.Amount * ol.Product.Price),
                    OrderLines = o.OrderLines.Select(ol => new
                    {
                        ol.Id,
                        ProductName = ol.Product.Name,
                        ol.Product.Price,
                        ol.Amount,
                        LineTotal = ol.Amount * ol.Product.Price
                    }).ToList()
                })
                .ToListAsync();

            return Ok(orders);
        }

        /// <summary>
        /// Gets a specific order by id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.user)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.DateTime,
                    CustomerName = o.user.UserName,
                    CustomerEmail = o.user.Email,
                    TotalAmount = o.OrderLines.Sum(ol => ol.Amount * ol.Product.Price),
                    OrderLines = o.OrderLines.Select(ol => new
                    {
                        ol.Id,
                        ProductName = ol.Product.Name,
                        ol.Product.Price,
                        ol.Amount,
                        LineTotal = ol.Amount * ol.Product.Price
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        /// <summary>
        /// Updates a specific order
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            // Handle order lines updates
            foreach (var orderLine in order.OrderLines)
            {
                if (orderLine.Id == 0)
                {
                    _context.OrderLines.Add(orderLine);
                }
                else
                {
                    _context.Entry(orderLine).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (order.OrderLines == null || !order.OrderLines.Any())
            {
                return BadRequest("Order must contain at least one order line");
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Reload the order with all related data
            var createdOrder = await _context.Orders
                .Include(o => o.user)
                .Include(o => o.OrderLines)
                    .ThenInclude(ol => ol.Product)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrder);
        }

        /// <summary>
        /// Deletes a specific order
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Remove order lines first
            _context.OrderLines.RemoveRange(order.OrderLines);
            
            // Then remove the order
            _context.Orders.Remove(order);
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
} 