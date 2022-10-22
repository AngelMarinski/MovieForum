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
    public class UpdateMovieAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;
        public UpdateMovieAsync()
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
        public async Task UpdateAsync_Should_UpdateMovie_When_ParamsValid()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.AddRangeAsync(Helper.Genres);
            await context.AddRangeAsync(Helper.MovieActors);

            await context.SaveChangesAsync();

            var movieID = Helper.Movies[1].Id;

            var Cast = _mapper.Map<ICollection<MovieActorDTO>>(Helper.MovieActors.Where(x => x.MovieId == movieID));
            var Tags = _mapper.Map<ICollection<MovieTagsDTO>>(Helper.MovieTags.Where(x => x.MovieId == movieID));

            var expected = new MovieDTO
            {
                Title = "Some Random Test Title Long Enough",
                Content = "Some Random Test Content Long Enough",
                ReleaseDate = DateTime.Today,
                Posted = DateTime.Now,
                GenreId = Helper.Genres[0].Id,
                ImagePath = "~/Images/random.png",
                Tags = Tags,
                Cast = Cast,
            };

            var service = new MoviesServices(context, _mapper);

            var actual = await service.UpdateAsync(movieID, expected);

            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.Content, actual.Content);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Movie existance validation failed!")]
        public async Task UpdateAsync_Should_ThrowException_When_MovieIDIsInvalid()
        {
            await context.AddRangeAsync(Helper.Movies);

            await context.SaveChangesAsync();

            var movieID = int.MinValue;

            var expected = new MovieDTO
            {
                Title = "Some Random Test Title Long Enough",
                Content = "Some Random Test Content Long Enough",
                ReleaseDate = DateTime.Today,
                Posted = DateTime.Now,
                GenreId = Helper.Genres[0].Id,
                ImagePath = "~/Images/random.png",
            };

            var service = new MoviesServices(context, _mapper);

            await service.UpdateAsync(movieID, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Genre existance validation failed!")]
        public async Task UpdateAsync_Should_ThrowException_When_GenreIDIsInvalid()
        {
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var movieID = Helper.Movies[1].Id;

            var Cast = _mapper.Map<ICollection<MovieActorDTO>>(Helper.MovieActors.Where(x => x.MovieId == movieID));
            var Tags = _mapper.Map<ICollection<MovieTagsDTO>>(Helper.MovieTags.Where(x => x.MovieId == movieID));

            var expected = new MovieDTO
            {
                Title = "Some Random Test Title Long Enough",
                Content = "Some Random Test Content Long Enough",
                ReleaseDate = DateTime.Today,
                Posted = DateTime.Now,
                GenreId = int.MinValue,
                ImagePath = "~/Images/random.png",
                Tags = Tags,
                Cast = Cast,
            };

            var service = new MoviesServices(context, _mapper);
            
            await service.UpdateAsync(movieID, expected);
        }
    }
}
