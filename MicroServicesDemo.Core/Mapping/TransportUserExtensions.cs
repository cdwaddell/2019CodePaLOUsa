using MicroServicesDemo.DataContracts;
using Newtonsoft.Json;

namespace MicroServicesDemo.Mapping
{
    public static class TransportUserExtensions
    {
        public static string ToSerializedTranportUser(this ITransportUser user)
        {
            var dto = new TransportUser
            {
                Bio = user.Bio,
                Image = user.Image,
                Username = user.Username
            };

            return JsonConvert.SerializeObject(dto);
        }

        public static T ToTransportUser<T>(this string serializedUser) where T : ITransportUser
        {
            return JsonConvert.DeserializeObject<T>(serializedUser);
        }

        public static string ToSerializedBlogTranportUser(this IBlogTransportUser user)
        {
            var dto = new BlogTransportUser
            {
                Bio = user.Bio,
                Image = user.Image,
                Username = user.Username,
                Following = user.Following
            };

            return JsonConvert.SerializeObject(dto);
        }

        public static ITransportUser ToBlogTransportUser<T>(this string serializedUser) where T : IBlogTransportUser
        {
            return JsonConvert.DeserializeObject<T>(serializedUser);
        }
    }
}
