using GlobalExceptionHandler;
using System.ComponentModel.DataAnnotations;

namespace Owleye.Controllers
{
    public class UserModel : BaseModel
    {
        [Required]
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        //public void Validate()
        //{
        //    var exception = new ValidationException("validation error", 400);
        //    if (string.IsNullOrEmpty(Username.Trim()))
        //    {
        //        exception.AddErrorToMessageList("UserName is Required");
        //    }
        //}
    }
}