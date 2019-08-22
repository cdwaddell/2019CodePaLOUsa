namespace MicroServiceDemo.Api.Auth.Models
{
    public class UserContainerDto<T> where T: SimpleUserBaseDto
    {
        public T User { get; set; }
    }
}