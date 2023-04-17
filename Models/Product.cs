
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace moore.Models
{
    public class Product 
    {   
        // this is product class initilize values for product model 

        //Pk
        [Key]
        public int ProductId { get; set; }
        //Requires user input
        [Required(ErrorMessage = "Please enter a product name.")]
        // the length of name shouldnt be more than 100 chars
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a product description.")]
        public string Description { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        //put the range for price value can not put more than this no expensive item 
        [Range(0, 9999999999999999.99, ErrorMessage = "Price must be between 0 and 9999999999999999.99.")]
        public double Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        //discound same as the above 
        [Range(0, 9999999999999999.99, ErrorMessage = "Discount must be between 0 and 9999999999999999.99.")]
        public double Discount { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Please enter the available quantity.")]
        //ignore - numbers as quantity
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a positive integer.")]
        public int Quantity { get; set; }
      
    }
}
