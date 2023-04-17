using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using moore.Data;
using moore.Models;

namespace moore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly mooreContext _context;

        public OrdersController(mooreContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            // Return all orders or a problem message if the order set is null
            return _context.Order != null ? 
                          View(await _context.Order.ToListAsync()) :
                          Problem("Entity set 'mooreContext.Order'  is null.");
        }
        [Authorize(Roles = "Admin")]
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Return a 404 error if no id is provided or the order set is null

            if (id == null || _context.Order == null)
            {
                //using custom error 
                return View("NotFound");
            }
            // Find the order with the specified id or return a 404 error if it doesn't exist
            var order = _context.Order
                    .FirstOrDefault(m => m.OrderID == id);

            if (order == null)
            {
                //using custom error 
                return View("NotFound");
            }
            // Return the order details view

            return View(order);
        }
        [Authorize(Roles = "Admin")]
        // GET: Orders/Create
        public IActionResult Create()
        {
            // Return the create order view
            ViewBag.Products = new SelectList(_context.Product, "ProductId", "Name");

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, int productId, int quantity,decimal price)
        {            // Check if the model state is valid

            if (ModelState.IsValid)
            {
                // Add the new order to the context and save changes

                _context.Add(order);
                await _context.SaveChangesAsync();
                // Redirect to the orders index view

                return RedirectToAction(nameof(Index));
            }
            // If the model state is not valid, return the create order view with the model

            return View(order);
        }
        [Authorize(Roles = "Admin")]
        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Return a 404 error if no ID is provided or the order set is null

            if (id == null || _context.Order == null)
            {
                return NotFound();
            }
            // Find the order with the specified ID or return a 404 error if it doesn't exist

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                //using custom error 
                return View("NotFound");
            }
            // Return the edit order view with the order model

            return View(order);
        }
     
        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,OrderDate,OrderTotal")] Order order)
        {
            // Return a 404 error if the ID provided does not match the order model's ID

            if (id != order.OrderID)
            {
                return NotFound();
            }
            // Check if the model state is valid

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the order in the context and try to save changes     
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // If the update fails due to concurrency, return a 404 error
                    if (!OrderExists(order.OrderID))
                    {

                        //using custom error 
                        return View("NotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                // Redirect to the orders index view

                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        [Authorize(Roles = "Admin")]
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            // Return a 404 error if no ID is provided or the order set is null

            if (id == null || _context.Order == null)
            {
                //using custom error 
                return View("NotFound");
            }
            // Find the order with the specified ID or return a 404 error if it doesn't exist

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                //using custom error 
                return View("NotFound");
            }
            // Return the delete order view with the order model

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the order with the specified ID and remove it from the context

            if (_context.Order == null)
            {
                return Problem("Entity set 'mooreContext.Order'  is null.");
            }

            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            // Redirect to the orders index view

            return RedirectToAction(nameof(Index));
        }
        // this method is helper checkes to see whether the order with the valid id exist or not this method then can be used in delete or edit 

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.OrderID == id)).GetValueOrDefault();
        }
        
    }
}
