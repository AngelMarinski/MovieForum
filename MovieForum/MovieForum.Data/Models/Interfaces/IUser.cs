using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Models.Interfaces
{
    internal interface IUser : IHasId
    {
        public string Name { get; set; }

        //Password
        //E-mail
        //First/Last NAme
        //UserName
        //Role 

    }
}
