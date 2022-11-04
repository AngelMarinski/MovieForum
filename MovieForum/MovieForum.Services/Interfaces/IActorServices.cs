using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Interfaces
{
    public interface IActorServices : ICRUDOperations<ActorDTO>
    {
        Task<Actor> GetActorById(int id);

        Task<Actor> GetActorByName(string firstName, string secondName);
    }
}
