namespace MicroServiceDemo.Api.Auth.Models
{
    public class UserRegisterDto : SimpleUserBaseDto
    {
        public string Password { get; set; }
    }
}