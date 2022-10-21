using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Services;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Tests.CommentServiceTests
{
    [TestClass]
    public class PostCommentAsync
    {

        private static IMapper _mapper;
        private MovieForumContext context;

        public PostCommentAsync()
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

        public async Task Should_CreateComment_When_CorrectDataIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                AuthorId = 3,
                MovieId = 1,
                Content = "Test content for comment post command!"
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();
            
            int count = await context.Comments.CountAsync();
           
            Assert.AreEqual(commentDTO.Content, res.Content);
            Assert.AreEqual(3, res.Id);
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_ShortContentIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                AuthorId = 3,
                MovieId = 1,
                Content = "T"
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_LongContentIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                AuthorId = 3,
                MovieId = 1,
                Content = new string('a',2001)
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_InvalidUserIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                AuthorId = -1,
                MovieId = 1,
                Content = "Test content for comment post command!"
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_InvalidMovieIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                AuthorId = 3,
                MovieId = -1,
                Content = "Test content for comment post command!"
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_NullAuthorIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                MovieId = 1,
                Content = "Test content for comment post command!"
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public async Task Should_Throw_When_NullContentIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                AuthorId = 3,
                MovieId = 1,
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_NullMovieIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                AuthorId = 3,
                Content = "Test content for comment post command!"
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_UserIsBlocked()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var commentDTO = new CommentDTO
            {
                AuthorId = 4,
                MovieId = 1,
                Content = "Test content for comment post command!"
            };

            var service = new CommentServices(context, _mapper);

            var res = await service.PostAsync(commentDTO);
            await context.SaveChangesAsync();

        }
    }
}
