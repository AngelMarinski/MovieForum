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
    public class DeleteCommentAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public DeleteCommentAsync()
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
        public async Task Should_Delete_Comment_IfCorrectDataIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var comment = await service.GetCommentByIdAsync(1);
            await service.DeleteAsync(1);

            var list = (List<CommentDTO>)await service.GetAsync();

            Assert.IsFalse(list.Contains(comment));
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_InvalidIdIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            await service.DeleteAsync(-4);
        }

    }
}
