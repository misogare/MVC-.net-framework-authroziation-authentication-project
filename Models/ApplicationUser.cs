using Microsoft.AspNetCore.Identity;



namespace   moore.Models
{
    public class ApplicationUser : IdentityUser
    {


        //this is the application user to be used by the entire project when the user is logged in so it shows the user's full name on current page at top or whenever the designing is meant for and using identity user as interface to get access to certain premissions for accounts
        public string FirstName { get; set; }
        public string LastName { get; set; }
                

        public string Address { get; set; }
        // this one is for subscription to see user paid or not when registering 
        public bool IsSubscriptionPaid { get; set; }




    }
}
