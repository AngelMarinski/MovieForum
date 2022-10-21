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

namespace MovieForum.Tests.TagServiceTests
{
    [TestClass]
    public class PostTagAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;
        public PostTagAsync()
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
        public async Task Should_PostTag_When_CorrectDataIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            var tagDTO = new TagDTO
            {
                TagName = "horror"
            };

            await service.PostAsync(tagDTO);

            var checkCount = (ICollection<TagDTO>)await service.GetAsync();

            Assert.AreEqual(3, checkCount.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_NullTagIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            TagDTO? tagDTO = null;

            await service.PostAsync(tagDTO);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_NullTagNameIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            var tagDTO = new TagDTO
            {
                
            };

            await service.PostAsync(tagDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_TagAlreadyExists()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            var tagDTO = new TagDTO
            {
                TagName = "drama"
            };

            await service.PostAsync(tagDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_ShortTagNameIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            var tagDTO = new TagDTO
            {
                TagName = "a"
            };

            await service.PostAsync(tagDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_LongTagNameIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            var tagDTO = new TagDTO
            {
                TagName = new string('a',21)
            };

            await service.PostAsync(tagDTO);
        }
    }
}
