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

namespace MovieForum.Tests.TagServiceTests
{
    [TestClass]
    public class GetTagAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;
        public GetTagAsync()
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
        public async Task Should_GetTag_When_CorrectIdIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var exp = Helper.Tags.FirstOrDefault(x => x.Id == 1); 

            var service = new TagServices(context, _mapper);

            var res = await service.GetTagByIdAsync(1);

            Assert.AreEqual(exp.TagName, res.TagName);
            Assert.AreEqual(exp.Id, res.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_IncorrectIdIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();
                     
            var service = new TagServices(context, _mapper);

            await service.GetTagByIdAsync(-1);
        }

        [TestMethod]
        public async Task Should_GetAllTags()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            var res = await service.GetAsync();

            Assert.AreEqual(2, res.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_ThereIsNoTags()
        {
            var service = new TagServices(context, _mapper);

            await service.GetAsync();
        }

        [TestMethod]
        public async Task Should_GetTag_When_CorrectNameIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var exp = Helper.Tags.FirstOrDefault(x => x.Id == 1);

            var service = new TagServices(context, _mapper);

            var res = await service.GetTagByNameAsync("drama");

            Assert.AreEqual(exp.TagName, res.TagName);
            Assert.AreEqual(exp.Id, res.Id);
        }
    }
}
