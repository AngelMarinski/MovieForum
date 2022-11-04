using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Services;
using MovieForum.Web.MappingConfig;
using System;
using System.Threading.Tasks;

namespace MovieForum.Tests.UserServiceTests
{
    [TestClass]
    public class BlockUserAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public BlockUserAsync()
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
        public async Task BlockUser_Should_BlockUserByID()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var userId = 1;

            var service = new UserServices(emailService, configuration,context, _mapper);

            await service.BlockUser(userId);

            Assert.IsTrue(context.Users.FirstOrDefaultAsync(x=> x.Id == userId).Result.IsBlocked == true);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task BlockUser_Should_ThrowException_When_UserIsBlocked()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var userId = 4;

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.BlockUser(userId);
        }

        [TestMethod]
        public async Task UnblockUser_Should_BlockUserByID()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var userId = 4;

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.UnblockUser(userId);

            Assert.IsTrue(context.Users.FirstOrDefaultAsync(x => x.Id == userId).Result.IsBlocked == false);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UnblockUser_Should_ThrowException_When_UserIsBlocked()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var userId = 1;

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.UnblockUser(userId);
        }
    }
}
