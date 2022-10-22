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

namespace MovieForum.Tests.UserServiceTests
{
    [TestClass]
    public class UpdateAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public UpdateAsync()
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
        public async Task UpdateAsync_Should_UpdateUserInfo()
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
                PhoneNumber = "+86 800 555 1234"
            };

            var service = new UserServices(context, _mapper);

            var actual = await service.UpdateAsync(1, user);

            Assert.IsTrue(actual.Email == user.Email);
            Assert.IsTrue(actual.FirstName == user.FirstName);
            Assert.IsTrue(actual.LastName == user.LastName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateAsync_Should_ThrowException_When_EmailExists()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var user = new UpdateUserDTO
            {
                Username = "fooBarUsername",
                FirstName = "Patkan",
                LastName = "Slavchev",
                Email = Helper.Users[1].Email,
                Password = "1234asdfgh",
                PhoneNumber = "+86 800 555 1234"
            };


            var service = new UserServices(context, _mapper);

            await service.UpdateAsync(1, user);
        }
    }
}
