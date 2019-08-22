namespace MicroServiceDemo.Api.Auth.Models
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginContainerDto
    {
        public UserLoginDto User { get; set; }
    }
}