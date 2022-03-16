using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MYSQLStoreAPI.Models
{
    
    public class Category
    {
        [Key]
        public int Category_id {get; set;}

        [Required]
        [Column(TypeName ="varchar(64)",Order = 1)]
        public string Category_name {get; set;}

        [Required]
        [Column(Order = 2)]
        public int Category_status {get; set;}
    }
}