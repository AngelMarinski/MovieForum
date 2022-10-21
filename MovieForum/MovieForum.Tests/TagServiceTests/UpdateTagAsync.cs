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
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Tests.TagServiceTests
{
    [TestClass]
    public class UpdateTagAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;
        public UpdateTagAsync()
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
        public async Task Should_UpdateTag_When_CorrectDataIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);

            var tagDTO = new TagDTO
            {
                TagName = "horror"
            };

            await service.UpdateAsync(1,tagDTO);

            var res = context.Tags.FirstOrDefault(x => x.Id == 1);

            Assert.AreEqual(tagDTO.TagName, res.TagName);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task Should_Throw_When_IncorrectTagIdIsPassed()
        {
            await context.AddRangeAsync(Helper.Tags);
            await context.SaveChangesAsync();

            var service = new TagServices(context, _mapper);
            var tagDTO = new TagDTO
            {
                TagName = "horror"
            };
            await service.UpdateAsync(-1,tagDTO);

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
                TagName = null
            };

            await service.UpdateAsync(1,tagDTO);
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

            await service.UpdateAsync(1,tagDTO);
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
                TagName = new string('a', 21)
            };

            await service.UpdateAsync(1,tagDTO);
        }
    }
}

