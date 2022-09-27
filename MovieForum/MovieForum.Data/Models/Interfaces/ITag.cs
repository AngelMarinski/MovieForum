using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Data.Models.Interfaces
{
    interface ITag : IHasId
    {
        public string Tag { get; set; }
    }
}
