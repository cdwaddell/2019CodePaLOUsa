using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Data.Entities;
using MicroServicesDemo.WebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Blog.Controllers.API.V1
{
    public class ProfilesController: ApiControllerBase
    {
        private readonly IBlogDbContext _context;

        public ProfilesController(IBlogDbContext context)
        {
            _context = context;
        }

        [HttpPost("{username}/follow")]
        public async Task<ActionResult> Follow([FromRoute]string username, CancellationToken cancellationToken) 
        {
            var currentUser = User.FindFirst("sub")?.Value;
            if (currentUser == null)
                return Forbid();
            var followedQuery = _context.FollowingUsers
                .Where(x => x.FollowedUser.Username == username);

            var followed = await _context.Users
                .Where(x => x.Username == currentUser)
                .Join(followedQuery, x => x.Id, x => x.UserId, (user, followingUser) => new
                {
                    User = user,
                    FollowingUser = followingUser
                }).SingleOrDefaultAsync(cancellationToken);

            if(followed.FollowingUser == null)
            {
                var toFollow = await _context.Users
                    .SingleOrDefaultAsync(x => x.Username == username, cancellationToken);

                if (toFollow == null)
                    return Ok();

                await _context.FollowingUsers
                    .AddAsync(new FollowingUser
                    {
                        UserId = followed.User.Id,
                        FollowedUserId = toFollow.Id
                    }, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Ok();
        }

        [HttpDelete("{username}/follow")]
        public async Task<ActionResult> Unfollow([FromRoute]string username, CancellationToken cancellationToken) 
        {
            var currentUser = User.FindFirst("sub")?.Value;
            if (currentUser == null)
                return Forbid();
            var followedQuery = _context.FollowingUsers
                .Where(x => x.FollowedUser.Username == username);

            var followed = await _context.Users
                .Where(x => x.Username == currentUser)
                .Join(followedQuery, x => x.Id, x => x.UserId, (user, followingUser) => new
                {
                    User = user,
                    FollowingUser = followingUser
                }).SingleOrDefaultAsync(cancellationToken);
            
            if(followed.FollowingUser != null)
            {
                _context.FollowingUsers.Remove(followed.FollowingUser);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromRoute]string username) {
            throw new NotImplementedException();
        }
    }
}
