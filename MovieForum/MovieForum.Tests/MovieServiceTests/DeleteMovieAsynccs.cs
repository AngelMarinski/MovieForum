using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Services;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Tests.MovieServiceTests
{
    [TestClass]
    public class DeleteMovieAsynccs
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public DeleteMovieAsynccs()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MovieForumProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [TestInitialize]
        public void Init()
        {
            var options = new DbContextOptionsBuilder<MovieForumContext>()
                               .UseInMemoryDatabase(Guid.NewGuid().ToString())
                               .Options;

            MovieForumContext movieForumContext = new MovieForumContext(options);
            context = movieForumContext;
        }

        [TestMethod]
        public async Task DeleteAsync_Should_DeletePost()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var service = new MoviesServices(context, _mapper);

            var movie = Helper.Movies[1];

            await service.DeleteAsync(movie.Id);

            movie = await this.context.Movies.FirstOrDefaultAsync(x => x.Id == movie.Id);

            Assert.IsTrue(movie.IsDeleted == true);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Movie existance validation failed!")]
        public async Task DeleteAsync_Should_ThrowException_When_MovieIDIsInvalid()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var service = new MoviesServices(context, _mapper);

            await service.DeleteAsync(int.MinValue);
        }
    }
}
