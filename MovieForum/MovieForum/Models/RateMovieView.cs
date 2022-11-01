using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieForum.Web.Models
{
    public class RateMovieView
    {
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public int Rate { get; set; }
    }
}
