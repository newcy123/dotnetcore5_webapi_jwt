using System.ComponentModel.DataAnnotations;

namespace MYSQLStoreAPI.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage ="กรุณากรอก Username")]
        public string Username {get; set;}

        [Required(ErrorMessage ="กรุณากรอก Password")]
        public string Password {get; set;}

        [EmailAddress]
        [Required(ErrorMessage ="กรุณากรอก Email")]
        public string Email {get; set;}
    }
}