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

namespace MovieForum.Tests.UserServiceTests
{
    [TestClass]
    public class GetUserAsync
    {
        private static IMapper _mapper;
        private MovieForumContext context;

        public GetUserAsync()
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
        public async Task GetUserIdAsync_Should_GetUserById()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var userId = 1;

            var expected = Helper.Users.FirstOrDefault(x => x.Id == userId);

            var service = new UserServices(context, _mapper);

            var actual = await service.GetUserAsync(userId);

            Assert.AreEqual(expected.Username, actual.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserIdAsync_Should_ThrowException_When_InvalidID()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var userId = int.MinValue;

            var service = new UserServices(context, _mapper);

            await service.GetUserAsync(userId);
        }


        [TestMethod]
        public async Task GetUserIdAsync_Should_GetUserByUsername()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var userId = 1;

            var expected = Helper.Users.FirstOrDefault(x => x.Id == userId);

            var service = new UserServices(context, _mapper);

            var actual = await service.GetUserAsync(expected.Username);

            Assert.AreEqual(expected.Id, actual.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserIdAsync_Should_ThrowException_When_InvalidUsername()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var username = "dummytext";

            var service = new UserServices(context, _mapper);

            await service.GetUserAsync(username);
        }

        [TestMethod]
        public async Task GetUserIdAsync_Should_GetUserByEmail()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var email = "adminsemail@gmail.com";

            var expected = Helper.Users.FirstOrDefault(x => x.Email == email);

            var service = new UserServices(context, _mapper);

            var actual = await service.GetUserByEmailAsync(email);

            Assert.AreEqual(expected.Username, actual.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserIdAsync_Should_ThrowException_When_InvalidEmail()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var email = "fOO@abv.bg";

            var service = new UserServices(context, _mapper);

            await service.GetUserByEmailAsync(email);
        }

        [TestMethod]
        public async Task UserCount_Should_ReturnAmountOfUsers()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(context, _mapper);

            Assert.AreEqual(Helper.Users.Count, service.UserCount());
        }

        [TestMethod]
        public async Task GetUserByIdAsync_Should_GetUserByID()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var id = 1;

            var expected = Helper.Users.FirstOrDefault(x => x.Id == id);

            var service = new UserServices(context, _mapper);

            var actual = await service.GetUserDTOByIdAsync(id);

            Assert.AreEqual(expected.Username, actual.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetUserByIdAsync_Should_ThrowException_When_InvalidID()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var service = new UserServices(context, _mapper);

            await service.GetUserDTOByIdAsync(int.MinValue);
        }

        [TestMethod]
        public async Task GetUserAllCommentsById_Should_ReturnAllUsersComments()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var userId = 1;

            var expected = context.Users.FirstOrDefaultAsync(x => x.Id == userId).Result.Comments;

            var service = new UserServices(context, _mapper);
             
            var actual = new List<CommentDTO>(await service.GetAllCommentsAsync(userId));

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserAllCommentsById_Should_ThrowException()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new UserServices(context, _mapper);

            await service.GetAllCommentsAsync(int.MinValue);
        }

        [TestMethod]
        public async Task GetUserAllCommentsByUsername_Should_ReturnAllUsersComments()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var username = "Maggie";

            var expected = Helper.Users.FirstOrDefault(x => x.Username == username).Comments;

            var service = new UserServices(context, _mapper);

            var actual = new List<CommentDTO>(await service.GetAllCommentsAsync(username));

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetUserAllCommentsByUsername_Should_ThrowException()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Comments);
            await context.SaveChangesAsync();

            var service = new UserServices(context, _mapper);

            await service.GetAllCommentsAsync("");
        }

        [TestMethod]
        public async Task GetAllMoviesAsyncById_Should_Return_AllMoviesFromUser()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var id = 1;

            var expected = context.Users.FirstOrDefault(x => x.Id == id).Movies;

            var service = new UserServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.GetAllMoviesAsync(id));

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public async Task GetAllMoviesAsyncByUsername_Should_Return_AllMoviesFromUser()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.AddRangeAsync(Helper.Movies);
            await context.SaveChangesAsync();

            var username = "Maggie";

            var expected = context.Users.FirstOrDefault(x => x.Username == username).Movies;

            var service = new UserServices(context, _mapper);

            var actual = new List<MovieDTO>(await service.GetAllMoviesAsync(username));

            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public async Task GetAsync_Should_ReturnAllUsers()
        {
            await context.AddRangeAsync(Helper.Users);
            await context.SaveChangesAsync();

            var expected = Helper.Users.ToList();

            var service = new UserServices(context, _mapper);

            var actual = new List<UserDTO>(await service.GetAsync());

            Assert.AreEqual(expected.Count, actual.Count);
        }
    }
}
