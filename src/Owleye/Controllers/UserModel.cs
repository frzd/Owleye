using GlobalExceptionHandler;
using System.ComponentModel.DataAnnotations;

namespace Owleye.Controllers
{
    public class UserModel : BaseModel
    {
        [Required]
        [StringLengthAttribute(maximumLength: 6, MinimumLength = 150)]
        [EmailAddressAttribute]
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}