using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Services;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Tests.TagServiceTests
{
    [TestClass]
    public class DeleteTagAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;
        public DeleteTagAsync()
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
        public async Task Should_DeleteTag_When_CorrectDataIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.AddRangeAsync(Helper.MovieTags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            var resTag = await service.DeleteAsync(1);

            var movieTag = context.MoviesTags.FirstOrDefault(x => x.TagId == resTag.Id);

            var res = (ICollection<TagDTO>)await service.GetAsync();

            Assert.AreEqual(1, res.Count);
            Assert.AreEqual(true, movieTag.IsDeleted);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_IncorrectDataIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            await service.DeleteAsync(-1);
        }
    }
}
