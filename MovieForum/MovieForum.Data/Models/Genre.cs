using MovieForum.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MovieForum.Data.Models
{
    public class Genre : IHasId, IDeletable
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public bool IsDeleted { get; set ; }
        public DateTime? DeletedOn { get; set; }
    }
}
