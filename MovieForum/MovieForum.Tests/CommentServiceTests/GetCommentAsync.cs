using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
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
    public class GetCommentAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;
        public GetCommentAsync()
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
        public async Task Should_GetCommentById()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var res = await service.GetCommentByIdAsync(1);
            var exp = _mapper.Map<CommentDTO>(Helper.Comments.FirstOrDefault(x => x.Id == 1));

            Assert.AreEqual(exp.Id, res.Id);
            Assert.AreEqual(exp.AuthorUsername, res.AuthorUsername);
            Assert.AreEqual(exp.Content, res.Content);


           
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_InvalidIdIsPassed()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);
            await service.GetCommentByIdAsync(6);

        }

        [TestMethod]
        public async Task Should_Return_ListOfCommentsIfThereIsAnyComments()
        {
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new CommentServices(context, _mapper);

            var result = (List<CommentDTO>)await service.GetAsync();

            Assert.AreEqual(service.CountComments(), result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_IfThereIsNoComments()
        {

            var service = new CommentServices(context, _mapper);

            await service.GetAsync();
        }


    }
}
