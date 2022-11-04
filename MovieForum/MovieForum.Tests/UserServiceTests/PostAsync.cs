using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieForum.Data;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using MovieForum.Services.Services;
using MovieForum.Web.MappingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Tests.UserServiceTests
{
    [TestClass]
    public class PostAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public PostAsync()
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
        public async Task PostAsync_Should_CreateUser()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "fooBarUsername",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = "foobarmail@gmail.com",
                Password = "1234asdfgh",
            };

            var numberOfUsers = Helper.Users.Count;

            var service = new UserServices(emailService, configuration, context, _mapper);

            var actual = await service.PostAsync(user);

            Assert.IsTrue(actual.Username == user.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task PostAsync_Should_ThrowException_When_EmailExists()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "fooBarUsername",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = "fakeemail@gmail.com",
                Password = "1234asdfgh",
            };

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.PostAsync(user);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task PostAsync_Should_ThrowException_When_UsernameExists()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "AngelMarinski",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = "mailmeangelo@gmail.com",
                Password = "1234asdfgh",
            };

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.PostAsync(user);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task PostAsync_Should_ThrowException_When_UsernameLenghtIsLess()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "foo",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = "foobarmail@gmail.com",
                Password = "1234asdfgh",
            };

            var service = new UserServices(emailService, configuration, context, _mapper);

            await service.PostAsync(user);
        }

    }
}
