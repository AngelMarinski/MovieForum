using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.DTOModels
{
    public class MovieActorDTO
    {
        public int? MovieId { get; set; }
        public MovieDTO Movie { get; set; }

        public int? ActorId { get; set; }
        public ActorDTO Actor { get; set; }
    }
}
