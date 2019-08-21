using MicroServiceDemo.Api.Blog.Data.Entities;
using MicroServiceDemo.Api.Blog.Security;
using MicroServicesDemo.Abstractions;

namespace MicroServiceDemo.Api.Blog.Abstractions
{
    /// <summary>
    /// A mapping interface for mapper cross cutting concerns
    /// </summary>
    public interface ISecurityMapper : IMapperBase
    {
        /// <summary>
        /// Map a User EF entity onto a security user
        /// </summary>
        /// <param name="src">The user</param>
        UserPrincipal MapUserPrincipal(User src);
    }
}