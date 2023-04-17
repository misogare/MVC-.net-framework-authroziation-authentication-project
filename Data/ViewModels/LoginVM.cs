using System.ComponentModel.DataAnnotations;

namespace moore.Data.ViewModels
{
    public class LoginVM
    {
        // this model is meant for Login when the user is redirected to this page , user will put in the values and information about itself
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       

    }
}
