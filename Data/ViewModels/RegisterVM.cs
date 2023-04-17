using System.ComponentModel.DataAnnotations;

namespace moore.Data.ViewModels
{
    public class RegisterVM
    {
        // this model is meant for registration when the user is redirected to this page , user will put in the values and information about itself 

        [Required(ErrorMessage = "Please enter a valid email")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a valid password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your Phone")]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter your Address")]
        [Display(Name = "Address")]
        public string Address{ get; set; }

        [Required(ErrorMessage = "Please choose if u are paid or unpaid sub")]
        [Display(Name = "Subscription")]
        public bool IsSubscriptionPaid { get; set; }




    }
}
