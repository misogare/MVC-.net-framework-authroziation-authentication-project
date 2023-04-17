using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace moore.Models
{
    public class OrderDetail
    {
        // this is the OrderDetail model to initilize values and be used in order Controller the point of interaction between product and order it contains 2 relations , and via these 2 order can access productID and vice versa 
        
        //key means its pk or pirmary key
        [Key]

        public int ID { get; set; }
        //required field used to say its required for the user to put input 
        [Required]
        public int Quantity { get; set; }
        //validation for price so its decimal 
        [Column(TypeName = "decimal(18, 2)")]
        [Required]
        public decimal Price { get; set; }
        //the first relation and fk which menas the order can access this class and it is the foreign key in this class
        [ForeignKey("Order")]
        // this class alose holds 2 IDs to be used by 2 other classes order and product : OrderID and ProductID
        public int OrderID { get; set; }
        public Order Order { get; set; }
        //the second relation and fk which menas the product can access this class and it is the foreign key in this class
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
