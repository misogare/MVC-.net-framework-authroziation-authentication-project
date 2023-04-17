using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using moore.Models;

namespace moore.Models
{
    public class Order
    {
        [Key]
        // this is the order model to initilize values for order class and use it in controller 

        
        public int OrderID { get; set; }

        // the date of creation of order 
        [Required]
        public DateTime OrderDate { get; set; }

        public double OrderTotal { get; set; }
        //not yet implemented 
        //public List<OrderDetail> OrderDetail { get; set; }
        //  public string UserId { get; set; }
        //[ForeignKey(nameof(UserId))]
        //  public ApplicationUser User { get; set; }


    }
}
