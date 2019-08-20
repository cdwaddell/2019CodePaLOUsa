using System;

namespace MicroServiceDemo.Api.Blog.Abstractions
{
    /// <summary>
    /// An DTO with attached audit data
    /// </summary>
    public interface IAuditableDto
    {

        /// <summary>
        /// The created date of the object
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// The last updated date of the object
        /// </summary>
        DateTime UpdatedAt { get; set; }
    }
}