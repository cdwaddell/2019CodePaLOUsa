namespace MicroServiceDemo.Api.Blog.Data.Entities
{
    /// <summary>
    /// A crosswalk entity for users to follow other users
    /// </summary>
    public class FollowingUser
    {
        /// <summary>
        /// The database assigned id of this entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The id of the user who is following another user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The id of the user who is being followed
        /// </summary>
        public int FollowedUserId { get; set; }

        /// <summary>
        /// A navigation property for the user who is following another user
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// A navigation property for the user who is being followed
        /// </summary>
        public virtual User FollowedUser { get; set; }
    }
}