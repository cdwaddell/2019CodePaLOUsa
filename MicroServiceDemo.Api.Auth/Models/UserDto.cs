using System;
using MicroServicesDemo.DataContracts;

namespace MicroServiceDemo.Api.Auth.Models
{
    public class UserDto : UserBaseDto, ITransportUser
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Token { get; set; }
    }
}