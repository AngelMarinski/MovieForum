using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Services;
using MovieForum.Services.DTOModels;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Tests.MovieServiceTests
{
    [TestClass]
    public class PostMovieAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public PostMovieAsync()
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
        public async Task PostAsync_For_Movie_Should_Create_Movie_When_ParamsValid()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Genres);
            await context.AddRangeAsync(Helper.Users);

            await context.SaveChangesAsync();

            var expected = new MovieDTO
            {
                AuthorId = Helper.Users[0].Id,
                Username = Helper.Users[0].Username,
                Title = "Some Random Test Title Long Enough",
                Content = "Some Random Test Content Long Enough",
                ReleaseDate = DateTime.Today,
                Posted = DateTime.Now,
                GenreId = Helper.Genres[0].Id,
                ImagePath = "~/Images/random.png"
            };

            var service = new MoviesServices(context, _mapper);

            var actual = await service.PostAsync(expected);

            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Content, actual.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "User validation failed!")]
        public async Task PostAsync_For_Movie_Should_ThrowException_When_UsernameIsInvalid()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Genres);
            await context.AddRangeAsync(Helper.Users);

            await context.SaveChangesAsync();

            string FakeUsername = "fakeusername123";

            var expected = new MovieDTO
            {
                AuthorId = Helper.Users[0].Id,
                Username = FakeUsername,
                Title = "Some Random Test Title Long Enough",
                Content = "Some Random Test Content Long Enough",
                ReleaseDate = DateTime.Today,
                Posted = DateTime.Now,
                GenreId = Helper.Genres[0].Id,
            };

            var service = new MoviesServices(context, _mapper);
            
            await service.PostAsync(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Genre validation failed!")]
        public async Task PostAsync_For_Movie_Should_ThrowException_When_GenreID_Is_Invalid()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Genres);
            await context.AddRangeAsync(Helper.Users);

            await context.SaveChangesAsync();

            int FakeGenreId = int.MinValue;

            var expected = new MovieDTO
            {
                AuthorId = Helper.Users[0].Id,
                Username = Helper.Users[0].Username,
                Title = "Some Random Test Title Long Enough",
                Content = "Some Random Test Content Long Enough",
                ReleaseDate = DateTime.Today,
                Posted = DateTime.Now,
                GenreId = FakeGenreId,
            };

            var service = new MoviesServices(context, _mapper);

            await service.PostAsync(expected);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "String lenghts validation has failed!")]
        public async Task PostAsync_For_Movie_Should_Create_Movie_When_TitleLenghtIsInvalid()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Genres);
            await context.AddRangeAsync(Helper.Users);

            await context.SaveChangesAsync();

            var expected = new MovieDTO
            {
                AuthorId = Helper.Users[0].Id,
                Username = Helper.Users[0].Username,
                Title = "d",
                Content = "d",
                ReleaseDate = DateTime.Today,
                Posted = DateTime.Now,
                GenreId = Helper.Genres[0].Id,
            };

            var service = new MoviesServices(context, _mapper);

            await service.PostAsync(expected);
        }
    }
}
