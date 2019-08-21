namespace MicroServiceDemo.Api.Auth.Models
{
    public class UserBaseDto : SimpleUserBaseDto
    {
        public string Bio { get; set; }
        public string Image { get; set; }
    }
}