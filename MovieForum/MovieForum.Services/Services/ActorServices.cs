using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieForum.Data;
using MovieForum.Data.Models;
using MovieForum.Services.DTOModels;
using MovieForum.Services.Helpers;
using MovieForum.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieForum.Services.Services
{
    public class ActorServices : IActorServices
    {
        private readonly MovieForumContext db;
        private readonly IMapper mapper;

        public ActorServices(MovieForumContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<ActorDTO>> GetAsync()
        {
            var actors = await this.db.Actors.Where(x => x.IsDeleted == false).ToListAsync();

            return this.mapper.Map<IEnumerable<ActorDTO>>(actors);
        }
        public async Task<Actor> GetActorById(int id)
        {
            var actor = await this.db.Actors.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new InvalidOperationException(Constants.ACTOR_NOT_FOUND);

            return actor;
        }

        public async Task<Actor> GetActorByName(string firstName, string secondName)
        {
            var actor = await this.db.Actors.FirstOrDefaultAsync(x => x.FirstName == firstName
            && x.LastName == secondName) 
                ?? throw new InvalidOperationException(Constants.ACTOR_NOT_FOUND);

            return actor;
        }

        public async Task<ActorDTO> DeleteAsync(int id)
        {
            var actor = await this.db.Actors.FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new InvalidOperationException(Constants.ACTOR_NOT_FOUND);

            actor.IsDeleted = true;
            actor.DeletedOn = DateTime.Now;

            await db.SaveChangesAsync();

            return mapper.Map<ActorDTO>(actor);
        }


        public async Task<ActorDTO> PostAsync(ActorDTO obj)
        {
            var actor = new Actor
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                IsDeleted = false,
            };

            await db.Actors.AddAsync(actor);
            await db.SaveChangesAsync();

            return mapper.Map<ActorDTO>(actor);
        }

        public async Task<ActorDTO> UpdateAsync(int id, ActorDTO obj)
        {
            var actor = await this.GetActorById(id);

            if(obj == null)
            {
                throw new InvalidOperationException("Actor has no value!");
            }

            actor.FirstName = obj.FirstName ?? actor.FirstName;
            actor.LastName = obj.LastName ?? actor.LastName;

            await db.SaveChangesAsync();

            return mapper.Map<ActorDTO>(actor);
        }

    }
}
