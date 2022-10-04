using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.Models
{
    public class MovieQueryParameters
    {
        public string Name { get; set; }
        public int? MinRating { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public string Username { get; set; }
    }
}
