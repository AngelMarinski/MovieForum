using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
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
    public class UpdateCommentAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public UpdateCommentAsync()
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
        public async Task Should_Update_Comment()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var commentDTO = new CommentDTO
            {
                Content = "This is update test content!"
            };
            var result = await service.UpdateAsync(1, commentDTO);

            Assert.AreEqual($"{commentDTO.Content} (Edited)", result.Content);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_ShortContentIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var commentDTO = new CommentDTO
            {
                Content = "s"
            };
            await service.UpdateAsync(1, commentDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_LongContentIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var commentDTO = new CommentDTO
            {
                Content = new string('a', 2001)
            };
            await service.UpdateAsync(1, commentDTO);
        }
    }
}
