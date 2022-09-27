using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Models.Interfaces
{
    public interface ITag : IHasId
    {
        public string TagName { get; set; }
    }
}
