using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYSQLStoreAPI.Models
{
    public class Products
    {
        [Key]
        public int Product_id {get; set;}

        [Required]
        [Column(TypeName ="varchar(64)",Order = 1)]
        public string Product_name {get; set;}

        [Required]
        [Column(TypeName ="decimal(10,2)",Order = 2)]
        public decimal Product_unitprice {get; set;}

        [Required]
        [Column(Order = 3)]
        public int Product_unit_stock {get; set;}

        [Required]
        [Column(Order = 4)]
        public string Product_picture {get; set;}

        [Column(Order = 5)]
        public DateTime Product_created_Date {get; set;} = DateTime.Now;

        
        [Column(Order = 6)]
        public DateTime ModifiedDate {get; set;} = DateTime.Now;

        [ForeignKey("Category_FK")]
        public int Category_id {get; set;}

        public virtual Category Category_FK {get; set;}
    }
}