namespace MicroServiceDemo.Api.Auth.Models
{
    public class UserUpdateDto : UserBaseDto
    {
        public string Password { get; set; }
    }
}