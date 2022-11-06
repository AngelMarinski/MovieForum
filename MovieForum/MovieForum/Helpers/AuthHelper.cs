using Microsoft.AspNetCore.Identity;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Web.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IUserServices userServices;
        private static readonly string ErrorMassage = "Invalid authentication info";

        public AuthHelper(IUserServices userServices)
        {
            this.userServices = userServices;
        }

        public async Task<User> TryLogin(string credential, string password)
        {
            try
            {
                var user = new User();
                 
                if (credential.Contains('@'))
                {
                     user = await userServices.GetUserByEmailAsync(credential);
                }
                else
                {
                    user = await userServices.GetUserAsync(credential);
                }

                if (user.IsEmailConfirmed)
                {

                    var passHasher = new PasswordHasher<User>();
                    var result = passHasher.VerifyHashedPassword(user, user.Password, password);

                    if (result != PasswordVerificationResult.Success)
                    {
                        throw new AuthenticationException();
                    }

                    return user;
                }

                user = null;
                return user;
            }
            catch (Exception)
            {
                throw new Exception(ErrorMassage);
            }
        }
    }
}
