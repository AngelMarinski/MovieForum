using System;
using System.Collections.Generic;
using System.Text;

namespace MovieForum.Services.Helpers
{
    public static class Constants
    {
        //Constraints
        public const int MOVIE_TITLE_MIN_LENGHT = 16;
        public const int MOVIE_TITLE_MAX_LENGHT = 64;
        public const int MOVIE_CONTENT_MIN_LENGHT = 32;
        public const int MOVIE_CONTENT_MAX_LENGHT = 8192;

        public const int USER_FIRSTNAME_MIN_LENGTH = 4;
        public const int USER_FIRSTNAME_MAX_LENGTH = 32;
        public const int USER_LASTNAME_MIN_LENGTH = 4;
        public const int USER_LASTNAME_MAX_LENGTH = 32;
        public const int USER_USERNAME_MIN_LENGTH = 4;
        public const int USER_PASSWORD_MIN_LENGTH = 8;

        //Messages
        public const string MOVIE_NOT_FOUND = "Movie not found!";
        public const string USER_NOT_FOUND = "User not found!";
        public const string COMMENT_NOT_FOUND = "Comment not found!";
        public const string NO_COMMENTS_FOR_THIS_MOVIE = "There is no comments for this movie. You can be the first one, give your oppinion!";
        public const string INVALID_DATA = "Invalid data passed";
        public const string LIKED = "liked";
        public const string DISLIKED = "disliked";
    }
}
