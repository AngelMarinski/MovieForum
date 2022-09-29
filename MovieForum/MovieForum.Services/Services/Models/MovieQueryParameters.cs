using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.Services.Models
{
    public class MovieQueryParameters
    {
        public string Name { get; set; }
        public int? MinRating { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string MostCommented { get; set; }
        public string MostRecent { get; set; }
    }
}
