using System;

namespace MicroServiceDemo.Api.Auth.Models
{
    public class UserDto : UserBaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Token { get; set; }
    }
}