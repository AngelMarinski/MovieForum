using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Models
{
    internal class Comment : IComment
    {
        public string Title { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
