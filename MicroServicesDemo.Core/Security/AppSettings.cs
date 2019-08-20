﻿namespace MicroServiceDemo.Api.Blog.Security
{
    /// <summary>
    /// Configuration settings for the application
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// The JWT signing key
        /// </summary>
        public string Secret { get; set; }
    }
}