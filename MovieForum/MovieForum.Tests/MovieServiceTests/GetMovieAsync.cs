using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Services;
using MovieForum.Services.DTOModels;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Tests.MovieServiceTests
{
    [TestClass]
    public class GetMovieAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public GetMovieAsync()
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
        public async Task Should_Get_All_Movies()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.GetAsync());
            var expected = new List<MovieDTO>(_mapper.Map<IEnumerable<MovieDTO>>(Helper.Movies));

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public async Task GetById_Should_Get_Movie_ById()
        {
            int id = 1;
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var service = new MoviesServices(context, _mapper);

            var actual = await service.GetByIdAsync(id);
            var expected = _mapper.Map<MovieDTO>(Helper.Movies.FirstOrDefault(x => x.Id == id));

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Invalid verification")]
        public async Task GetById_Should_ThrowException_When_Id_Invalid()
        {
            int id = int.MaxValue - 1;
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var service = new MoviesServices(context, _mapper);

            await service.GetByIdAsync(id);
        }

        [TestMethod]
        public async Task GetMostRecentPostsAsync_Should_Return_MostRecentPosts()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var expected = await context.Movies.OrderByDescending(x => x.Posted)
                                        .Take(10).ToListAsync();

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.GetMostRecentPostsAsync());

            Assert.AreEqual(expected[0].Id, actual[0].Id);
            Assert.AreEqual(expected[0].Title, actual[0].Title);
            Assert.AreEqual(expected[0].Content, actual[0].Content);
        }

        [TestMethod]
        public async Task GetTopCommentedAsync_Should_Return_MostCommentedPosts()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var expected = await context.Movies.OrderByDescending(x => x.Comments.Count)
                                        .Take(10).ToListAsync();

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.GetTopCommentedAsync());

            Assert.AreEqual(expected[0].Id, actual[0].Id);
            Assert.AreEqual(expected[0].Title, actual[0].Title);
            Assert.AreEqual(expected[0].Content, actual[0].Content);
        }
    }
}
