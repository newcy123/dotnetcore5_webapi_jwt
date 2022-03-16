using System.ComponentModel.DataAnnotations;

namespace MYSQLStoreAPI.Authentication
{
    public class LoginModel
    {   

        [Required(ErrorMessage ="กรุณากรอก Username")]
        public string Username {get; set;}

        [Required(ErrorMessage ="กรุณากรอก Password")]
        public string Password {get; set;}
    }
}