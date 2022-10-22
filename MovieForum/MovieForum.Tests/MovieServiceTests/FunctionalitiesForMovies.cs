using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Services;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Models;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Tests.MovieServiceTests
{
    [TestClass]
    public class FunctionalitiesForMovies
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public FunctionalitiesForMovies()
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
        public async Task FilterByAsync_Should_FilterMovies_ByGivenTitle()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var parameters = new MovieQueryParameters();
            parameters.Title = "Top";

            var expected = _mapper.Map<MovieDTO>(Helper.Movies.FirstOrDefault(x => x.Title.Contains(parameters.Title)));

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.FilterByAsync(parameters));

            Assert.IsTrue(actual.Any(x => x.Id == expected.Id
                           && x.Title == expected.Title));
        }

        [TestMethod]
        public async Task FilterByAsync_Should_FilterMovies_ByGivenTitleAndMinRating()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Ratings);

            await context.SaveChangesAsync();

            var parameters = new MovieQueryParameters();
            parameters.Title = "Top";
            parameters.MinRating = 7;

            var expected = _mapper.Map<MovieDTO>(Helper.Movies.FirstOrDefault(x => x.Title.Contains(parameters.Title)));

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.FilterByAsync(parameters));

            Assert.IsTrue(actual.Any(x => x.Id == expected.Id
                           && x.Title == expected.Title));
        }

        [TestMethod]
        public async Task FilterByAsync_Should_FilterMovies_ByGivenTitleAndUser()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Users);

            await context.SaveChangesAsync();

            var parameters = new MovieQueryParameters();
            parameters.Title = "Top";
            parameters.Username = "AngelMarinski";

            var expected = _mapper.Map<MovieDTO>(Helper.Movies.FirstOrDefault(x => x.Title.Contains(parameters.Title)));

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.FilterByAsync(parameters));

            Assert.IsTrue(actual.Any(x => x.Id == expected.Id
                           && x.Title == expected.Title
                           && x.AuthorId == expected.AuthorId));
        }

        [TestMethod]
        public async Task FilterByAsync_Should_SortMovies_ByTitleDesc()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var parameters = new MovieQueryParameters();
            parameters.SortBy = "title";
            parameters.SortOrder = "desc";

            var expected = _mapper.Map<List<MovieDTO>>(Helper.Movies.OrderByDescending(x => x.Title));

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.FilterByAsync(parameters));

            Assert.IsTrue(expected.Count == actual.Count
                && expected[0].Id == actual[0].Id
                && expected[0].Title == actual[0].Title);
        }


        [TestMethod]
        public async Task FilterByAsync_Should_SortMovies_ByReleaseDate()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var parameters = new MovieQueryParameters();
            parameters.SortBy = "releasedate";

            var expected = _mapper.Map<List<MovieDTO>>(Helper.Movies.OrderBy(x => x.ReleaseDate));

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.FilterByAsync(parameters));

            Assert.IsTrue(expected.Count == actual.Count
                && expected[0].Id == actual[0].Id
                && expected[0].Title == actual[0].Title);
        }

        [TestMethod]
        public async Task FilterByAsync_Should_SortMovies_ByRating()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Ratings);

            await context.SaveChangesAsync();

            var parameters = new MovieQueryParameters();
            parameters.SortBy = "rating";

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.FilterByAsync(parameters));

            Assert.IsTrue(actual[0].Rating > actual[1].Rating);
        }


        [TestMethod]
        public async Task FilterByAsync_Should_SortMovies_ByCommentsCount()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var parameters = new MovieQueryParameters();
            parameters.SortBy = "comments";

            var expected = _mapper.Map<List<MovieDTO>>(Helper.Movies.OrderByDescending(x => x.Comments.Count));

            var service = new MoviesServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.FilterByAsync(parameters));

            Assert.IsTrue(expected.Count == actual.Count
                && expected[0].Id == actual[0].Id
                && expected[0].Title == actual[0].Title);
        }

        [TestMethod]
        public async Task AddTagAsync_Should_AddTagToTheMovie()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Tags);

            await context.SaveChangesAsync();

            var movieId = 1;
            var tagName = "drama";

            var service = new MoviesServices(context, _mapper);

            var actual = await service.AddTagAsync(movieId, tagName);

            Assert.IsTrue(actual.Tags.Any(x=>x.TagName == tagName));
        }

        [TestMethod]
        public async Task AddTagAsync_Should_CreateTagAndAddTagToTheMovie()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Tags);

            await context.SaveChangesAsync();

            var movieId = 1;
            var tagName = "sci-fi";

            var service = new MoviesServices(context, _mapper);

            var actual = await service.AddTagAsync(movieId, tagName);

            Assert.IsTrue(actual.Tags.Any(x => x.TagName == tagName));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Movie existance validation failed!")]
        public async Task AddTagAsync_Should_ThrowExceptionWhenInvalidMovieIDPassed()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var movieId = int.MinValue;
            var tagName = "sci-fi";

            var service = new MoviesServices(context, _mapper);

            await service.AddTagAsync(movieId, tagName);
        }

        [TestMethod]
        public async Task RemoveTagAsync_Should_RemoveTagFromPost()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Tags);
            await context.AddRangeAsync(Helper.MovieTags);

            await context.SaveChangesAsync();

            var movieId = 1;
            var tagName = "action";

            var service = new MoviesServices(context, _mapper);

            var actual = await service.RemoveTagAsync(movieId, tagName);

            Assert.IsFalse(actual.Tags.Any(x => x.TagName == tagName));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Movie existance validation failed!")]
        public async Task RemoveTagAsync_Should_ThrowExceptionWhenInvalidMovieIDPassed()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var movieId = int.MinValue;
            var tagName = "sci-fi";

            var service = new MoviesServices(context, _mapper);

            await service.RemoveTagAsync(movieId, tagName);
        }

        [TestMethod]
        public async Task RateMovieAsync_Should_AddRating()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Users);

            await context.SaveChangesAsync();

            var movieId = 1;
            var userId = 1;
            var rate = 10;

            var service = new MoviesServices(context, _mapper);

            var expected = await service.GetByIdAsync(movieId);

            var actual = await service.RateMovieAsync(movieId, userId, rate);

            Assert.IsTrue(actual.Rating.CompareTo(expected.Rating) > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Movie existance validation failed!")]
        public async Task RateMovieAsync_Should_ThrowException_When_MovieIdInvalid()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Users);

            await context.SaveChangesAsync();

            var movieId = int.MinValue;
            var userId = 1;
            var rate = 10;

            var service = new MoviesServices(context, _mapper);
            await service.RateMovieAsync(movieId, userId, rate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "User existance validation failed!")]
        public async Task RateMovieAsync_Should_ThrowException_When_UserIdInvalid()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Users);

            await context.SaveChangesAsync();

            var movieId = 1;
            var userId = int.MinValue;
            var rate = 10;

            var service = new MoviesServices(context, _mapper);
            await service.RateMovieAsync(movieId, userId, rate);
        }
    }
}
