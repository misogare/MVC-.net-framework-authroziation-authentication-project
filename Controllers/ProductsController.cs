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
// This code defines a controller class for the Products model in an ASP.NET Core MVC web application.
// The controller handles requests and responses related to CRUD operations on the Product model
namespace moore.Controllers
{
    public class ProductsController : Controller
    {

        private readonly mooreContext _context;
        // Constructor that initializes a mooreContext instance
       

        public ProductsController(mooreContext context)
        {
            _context = context;
        }

        // GET: Products
        // Returns a view of all products or an error message if the product entity set is null
      
        // Returns the details view of a specific product by its ID or a 404 error if not found
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Admin")]

        // GET: Products/Create
        // Returns the view for creating a new product

        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        // POST: Products/Create
        // Creates a new product in the database and redirects to the Index view or returns the Create view if model state is invalid
        // Protects against overposting attacks by only binding specific propert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,Price,Discount,DateCreated,Quantity")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]
        // GET: Products/Edit/5
        // Returns the view for editing a specific product by its ID or a 404 error if not found
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]

        // POST: Products/Edit/5
        // Edits a product in the database and redirects to the Index view or returns the Edit view if model state is invalid
        // Protects against overposting attacks by only binding specific properties
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,Price,Discount,DateCreated,Quantity")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [Authorize(Roles = "Admin")]

        // GET: Products/Delete/5
        // Returns the view for deleting a specific product by its ID or a 404 error if not found
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Admin")]

        // POST: Products/Delete/5
        // Deletes a product from the database and redirects to the Index view
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'mooreContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
        // This code is responsible for filtering the products based on the search string entered by the user in the search bar.
        // If the 'Product' entity set is null, it will return an error message using the 'Problem' method.
        public async Task<IActionResult> Filter(string searchString)
        {
            if (_context.Product == null)
            {
                return Problem("product'  is null.");
            }
            var product = from n in _context.Product
                          select n;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(p => p.Name!.Contains(searchString));

            }

            return View("index", product);
        }
        // This code is responsible for displaying the products based on the user's subscription type.
        // It first checks if the user is authenticated and has a valid subscription.
        // If the subscription is not paid, it shows only the products with "Host only ( use it on your own device)" in the description so the software or product only can run on their system.
        // If the subscription is paid, it shows only the products without "Host only ( use it on your own device)" in the description so that means it can be on server or cloud based product.
        // If the user is not authenticated, it redirects the user to the login page.


        public async Task<IActionResult> Index()
        {
           
           

            var products =  _context.Product.ToList();
            if (User.Identity.IsAuthenticated)
            {

                if (!User.Claims.Any(c => c.Type == "SubscriptionType" && c.Value == "Paid") && User.IsInRole("Admin"))
                {
                    //admin views everything in product page
                    products = products.ToList();

                }

                else if (!User.Claims.Any(c => c.Type == "SubscriptionType" && c.Value == "Paid"))
                {
                    // If subscription is not paid, show only products with "Host" in the description
                    products = products.Where(p => p.Description.Contains("Host only ( use it on your own device)")).ToList();
                }
                else
                {
                    // If subscription ispaid, show only products with not "Host" in the description
                    products = products.Where(p => !p.Description.Contains("Host only ( use it on your own device)")).ToList();

                    
                }
                // Return the 'Index' view with the filtered products.

                return View(products);
            }
            else
            {
                // If the user is not authenticated, redirect to the login page.


                return RedirectToAction("Login", "Account");

            }



                
        }
    }
}
