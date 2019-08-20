using System;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Blog.Data.Entities
{
    /// <summary>
    /// An entity for assigning audit data
    /// </summary>
    [Owned]
    public class AuditRecord
    {
        /// <summary>
        /// The created date of the entity
        /// </summary>
        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;

        /// <summary>
        /// The last updated date of the entity
        /// </summary>
        public DateTime UpdatedAt { get; set; }
            = DateTime.UtcNow;
    }
}