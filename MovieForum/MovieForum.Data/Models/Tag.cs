using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Models
{
    internal class Tag : ITag
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string ITag.Tag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
