﻿using AutoMapper;
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
    public class TagServices : ITagServices
    {
        private readonly MovieForumContext data;
        private readonly IMapper map;

        public TagServices(MovieForumContext data, IMapper map)
        {
            this.data = data;
            this.map = map;
        }

        public async Task<IEnumerable<TagDTO>> GetAsync()
        {
            var tags = await data.Tags.Where(x => x.IsDeleted == false).ToListAsync();

            if (tags.Count==0)
            {
                throw new InvalidOperationException(Constants.NO_TAGS_FOUND);
            }

            return map.Map<IEnumerable<TagDTO>>(tags);
        }

        public async Task<TagDTO> GetTagByIdAsync(int tagId)
        {
            var tag = await data.Tags.FirstOrDefaultAsync(x => x.Id == tagId && x.IsDeleted == false) ??
                throw new InvalidOperationException(Constants.NO_TAGS_FOUND);

            return map.Map<TagDTO>(tag);
        }

        public async Task<TagDTO> GetTagByNameAsync(string tagName)
        {
            var tag = await data.Tags.FirstOrDefaultAsync(x => x.TagName == tagName && x.IsDeleted == false) ??
                throw new InvalidOperationException(Constants.NO_TAGS_FOUND);

            return map.Map<TagDTO>(tag);
        }

        //public async Task<string> DeleteTagFromMovieAsync(int movieId, int tagId)
        //{
        //    var movie = await data.Movies.FirstOrDefaultAsync(x => x.Id == movieId && x.IsDeleted == false) ??
        //        throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

        //    var tag = await data.MoviesTags.FirstOrDefaultAsync(x => x.TagId == tagId) ??
        //        throw new InvalidOperationException(Constants.NO_TAGS_FOUND);

        //    var tagToDel = await data.Tags.FirstOrDefaultAsync(x => x.Id == tagId);
        //    tagToDel.IsDeleted = true;
        //    tagToDel.DeletedOn = DateTime.Now;

        //    return Constants.DELETED_TAG;
           
        //}
        //public async Task<string> DeleteAsync(int movieId, string tagName)
        //{
        //    var movie = await data.Movies.FirstOrDefaultAsync(x => x.Id == movieId && x.IsDeleted == false) ??
        //        throw new InvalidOperationException(Constants.MOVIE_NOT_FOUND);

        //    var tag = await data.MoviesTags.FirstOrDefaultAsync(x => x.Tag.TagName == tagName) ??
        //        throw new InvalidOperationException(Constants.NO_TAGS_FOUND);

        //    var tagToDel = await data.Tags.FirstOrDefaultAsync(x => x.TagName == tagName);
        //    tagToDel.IsDeleted = true;
        //    tagToDel.DeletedOn = DateTime.Now;

        //    return Constants.DELETED_TAG;

        //}

        public async Task<TagDTO> PostAsync(TagDTO obj)
        {
            var tag = await data.Tags.FirstOrDefaultAsync(x => x.TagName == obj.TagName);

            if (tag !=null)
            {
                throw new InvalidOperationException("This tag already exists!");
            }

            var newTag = new Tag
            {
                TagName = obj.TagName
            };

            await data.Tags.AddAsync(newTag);

            await data.SaveChangesAsync();

            return map.Map<TagDTO>(newTag);
        }

        public async Task<TagDTO> UpdateAsync(int id, TagDTO obj)
        {
            var tag = await data.Tags.FirstOrDefaultAsync(x => x.Id == id) ??
               throw new InvalidOperationException("This tag is not found!");

            if (obj.TagName!=null)
            {
                tag.TagName = obj.TagName;
            }
            else
            {
                throw new InvalidOperationException("No data to update a tag is passed!");
            }

            await data.SaveChangesAsync();

            return map.Map<TagDTO>(tag);
        }

        public async Task<TagDTO> DeleteAsync(int id)
        {
            var tag = await data.Tags.FirstOrDefaultAsync(x => x.Id == id) ??
                throw new InvalidOperationException("This tag is not found!");

            tag.IsDeleted = true;
            tag.DeletedOn = DateTime.Now;            

            await data.SaveChangesAsync();

            return map.Map<TagDTO>(tag);
        }
    }
}
