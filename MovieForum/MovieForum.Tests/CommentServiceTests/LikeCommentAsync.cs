using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Services.Services;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Tests.CommentServiceTests
{
    [TestClass]
    public class LikeCommentAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public LikeCommentAsync()
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
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_InvalidCommentIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            await service.LikeCommentAsync(-1, 1);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_InvalidUserIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            await service.LikeCommentAsync(1, -1);

        }

        [TestMethod]
        public async Task Should_LikeComment_When_UserDidNotEverReactedToThisComment()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var firstComment = context.Comments.FirstOrDefault(x=>x.Id == 1);

            await service.LikeCommentAsync(1, 1);

            Assert.AreEqual(1, firstComment.LikesCount);
            Assert.AreEqual(0, firstComment.DisLikesCount);
        }

        [TestMethod]
        public async Task Should_RemoveLike_When_UserAlreadyLikedComment()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var firstComment = context.Comments.FirstOrDefault(x => x.Id == 1);

            await service.LikeCommentAsync(1, 1);
            await service.LikeCommentAsync(1, 1);

            Assert.AreEqual(0, firstComment.LikesCount);
            Assert.AreEqual(0, firstComment.DisLikesCount);



        }

        [TestMethod]
        public async Task Should_RemoveDislikeAndAddLike_When_UserAlreadyDislikedComment()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var firstComment = context.Comments.FirstOrDefault(x => x.Id == 1);

            await service.DislikeCommentAsync(1, 1);
            await service.LikeCommentAsync(1, 1);

            Assert.AreEqual(1, firstComment.LikesCount);
            Assert.AreEqual(0, firstComment.DisLikesCount);



        }
    }
}

