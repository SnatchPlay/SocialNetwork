using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using SharpCompress.Common.Zip;
using Social.Models;
using Social.Repositories;

namespace Social.Logic
{
    public class PostLogic
    {
        public PostRepository repository;
        public RepositoryUser RepositoryUser;
        public PostLogic postLogic;
        public UserLogic userLogic;
        public PostLogic()
        {
            repository = new PostRepository();
            //postLogic = new PostLogic();
            RepositoryUser = new RepositoryUser();
        }
        //
        public void InsertPost(string text, string email)
        {
            Posts post = new Posts();
            post.Body = text;
            post.user = RepositoryUser.GetUserId(email);
            post.Date = DateTime.Now;
            repository.Add(post);
        }
        //

        public void AddLike(ObjectId postId)
        {
            repository.AddLike(postId);
        }


        public int GetLikes(ObjectId postID)
        {
            try
            {
                return repository.GetLike(postID);
            }
            catch
            {
                return 0;
            }

        }

        public bool AddComment(string email, string text, ObjectId postId)
        {

            Comments comment = new Comments();
            comment.Body = text;
            comment.User = RepositoryUser.GetUserId(email);
            comment.Date = Convert.ToString(DateTime.Now);
            try
            {
                repository.AddComment(comment, postId);
                return true;
            }
            catch
            {
                return false;
            }

        }


        public List<Posts> GetNewPosts(List<string> following)
        {
            List<ObjectId> ids = new List<ObjectId>();
            if (following != null)
            {
                foreach (var el in following)
                {
                    ids.Add(RepositoryUser.GetUserId(el));
                }
                return repository.GetNewPosts(following);
            }

            return new List<Posts>();

        }

        public List<Posts> GetPosts(string email)
        {
            List<Posts> posts = new List<Posts>();
            try
            {
                posts = repository.GetPosts(RepositoryUser.GetUserId(email));
                return posts;
            }
            catch
            {
                return posts;
            }

        }

        public Posts GetPost(ObjectId postId)
        {
            Posts post = new Posts();
            try
            {
                post = repository.GetPost(postId);
                return post;
            }
            catch
            {
                return post;
            }

        }
        public void PostReaction(string email)
        {
            List<Posts> list = GetPosts(email);
            UserLogic userLogic = new UserLogic();

            var posts = from p in list select p;
            foreach (Posts p in posts)
            {
                if (p.user == userLogic.GetUser(email).Id)
                {
                    Console.WriteLine($"{p.Body}\n{p.Date}\n");
                    Console.WriteLine("Do you want to react on posts?\n 1-yes\n 2-no ");
                    var x = Console.ReadLine();
                    switch (x)
                    {
                        case "1":
                            Console.WriteLine(" 1-like\n 2-comment");
                            var z = Console.ReadLine();
                            switch (z)
                            {
                                case "1":
                                    AddLike(p.Id);

                                    break;
                                case "2":
                                    Console.WriteLine(" Write you comment:");
                                    string y = Console.ReadLine();
                                    AddComment(email, y, p.Id);
                                    Console.WriteLine("Well Done!");

                                    break;
                            }
                            break;
                    }
                }
            }
        }
    }
}

