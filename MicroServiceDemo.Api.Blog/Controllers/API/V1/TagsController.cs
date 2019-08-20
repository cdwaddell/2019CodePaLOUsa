using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroServiceDemo.Api.Blog.Abstractions;
using MicroServiceDemo.Api.Blog.Data;
using MicroServiceDemo.Api.Blog.Models;
using MicroServiceDemo.Api.Blog.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MicroServiceDemo.Api.Blog.Controllers.API.V1
{
    /// <summary>
    /// API endpoints for managing tags
    /// </summary>
    public class TagsController : ApiControllerBase
    {
        private readonly BlogDbContext _context;
        private readonly IDtoMapper _mapper;

        /// <summary>
        /// Initializes a new TagsController
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="mapper">The automapper class</param>
        public TagsController(BlogDbContext context, IDtoMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all article tags
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>The list of tags in the system</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<TagsDto>> Get(CancellationToken cancellationToken)
        {
            return new TagsDto
            {
                Tags = _mapper.MapTags(
                    await _context.Tags.AsNoTracking().ToArrayAsync(cancellationToken)
                ).ToArray()
            };
        }
    }
}