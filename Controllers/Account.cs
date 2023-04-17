using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using moore.Models;
using moore.Data.ViewModels;
using moore.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;
using System.Security.Claims;


namespace moore.Controllers
{
    public class Account : Controller
    {
        // This controller is responsible for handling user authentication (login and registration)
        // and authorization (role-based access control).

        // Variable initialization
        private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly mooreContext _context;

        // Constructor for the Account controller that accepts UserManager, SignInManager and mooreContext as parameters
        public Account(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, mooreContext context)
            {
            // Store the parameters as instance variables for use within the controller
            _userManager = userManager;
                _signInManager = signInManager;
            _context = context;
        }
            //the login page  get method
            public IActionResult Login() => View(new LoginVM());
        //the login page post method
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            //model validation or values validation and upon succession login to the account 
            if (!ModelState.IsValid) return View(loginVM);
            // Find the user by their email address

            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            // If the user exists, attempt to sign them in with the provided password
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);// these 2 false are for remember me and lockout which we dont want in our testing
                    if (result.Succeeded)
                    {
                        // If the sign-in attempt is successful, redirect the user to the home page

                        return RedirectToAction("Index", "Home");
                    }
                }
                // If the password is incorrect, redisplay the login view with an error message

                TempData["Error"] = "password wrong. Please, try again!";
                return View(loginVM);
            }
            // If the user doesn't exist, redisplay the login view with an error message

            TempData["Error"] = "no email by this email. Please, try again!";
            return View(loginVM);
        }

     
          

        //register get method 
            public IActionResult Register() => View(new RegisterVM());
        //register post method  
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            //same thing as login model valid show the registervm view 
            if (!ModelState.IsValid) return View(registerVM);
            //if the user or email allready exist says the email is in database 
            var user = await _userManager.FindByEmailAsync(registerVM.Email);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerVM);
            }
            // Hash the user's password

            var passwordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, registerVM.Password);

            // Create a new ApplicationUser object with the values from the registration form
            var newUser = new ApplicationUser()
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                UserName = registerVM.Email,
                Email = registerVM.Email,
                // this one was a pain ,well normalize should be used becuase the indention and spacing appreantly happens 
                //when the user puts email as input in registration form so if you normalize that you can get the inputs from db and use it login page 
                NormalizedEmail = registerVM.Email,

                PhoneNumber = registerVM.Phone,
                Address = registerVM.Address,
                IsSubscriptionPaid = registerVM.IsSubscriptionPaid,
                PasswordHash = passwordHash

            };
            newUser.TwoFactorEnabled = false;
            newUser.PhoneNumberConfirmed = false;
            newUser.LockoutEnabled = false;
            newUser.AccessFailedCount = 0;
            newUser.EmailConfirmed = true;


            
            // Create the new user in the database

            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);



            if (newUserResponse.Succeeded)
                // If the user was created successfully, show the register compeleted message

                await _userManager.AddToRoleAsync(newUser, UserRole.User);
                 _context.AddRange(newUser);
                await _context.SaveChangesAsync();

                await _userManager.AddClaimAsync(newUser, new Claim("SubscriptionType", registerVM.IsSubscriptionPaid ? "Paid" : "Unpaid"));

            // show the completion message 
            return View("RegisterCompleted");
        }
   
        //log out post method it redirects to home page so no need of get method 
            [HttpPost]
            public async Task<IActionResult> Logout()
            {
            // Sign the user out

            await _signInManager.SignOutAsync();
            // Redirect the user to the home page

            return RedirectToAction("Index", "Home");
            }
        //upon denial of access run
            public IActionResult AccessDenied(string ReturnUrl)
            {
                return View();
            }
        // The following code is a controller that manages users in an application
        // The controller requires that the user be authorized with the "Admin" role to access its actions
        // The controller interacts with a database context named "_context"
        // This action returns a view with all the users in the database context

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
        // This action returns a view with details of a specific user based on the provided id

        [Authorize(Roles = "Admin")]
        // GET: Account/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Find the user with the specified ID or return a 404 error if it doesn't exist


            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // This action returns a view to delete a specific user based on the provided id

        [Authorize(Roles = "Admin")]
        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(String? id)
        {
            // Return a 404 error if no ID is provided or the order set is null

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            // Find the order with the specified ID or return a 404 error if it doesn't exist

            var order = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            // Return the delete order view with the order model

            return View(order);
        }
        // This action deletes a specific user based on the provided id
        // It is called when the user confirms the deletion in the delete view
        // POST: Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(String id)
        {
            // Find the order with the specified ID and remove it from the context

            if (_context.Users == null)
            {
                return Problem("Entity set 'mooreContext.Order'  is null.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            // Redirect to the orders index view

            return RedirectToAction(nameof(Users));
        }
        // This method checks if a user with the provided id exists in the context

        private bool UserExists(string id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
    }
