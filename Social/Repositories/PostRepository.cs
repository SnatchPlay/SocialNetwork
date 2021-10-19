using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using SharpCompress.Common.Zip;
using Social.Models;

namespace Social.Repositories
{
    public class PostRepository
    {
        static string connString = "mongodb://localhost:27017";
        static MongoClient client = new MongoClient(connString);
        IMongoDatabase database = client.GetDatabase("sc");
        IMongoCollection<Posts> postsCollection;
        public PostRepository()
        {
            ReadFromMongo();
        }

        public void ReadFromMongo()
        {
            postsCollection = database.GetCollection<Posts>("posts");
        }
        public void Add(Posts post) =>
          postsCollection.InsertOne(post);
        public void AddLike(ObjectId postId)
        {
            var filter = Builders<Posts>.Filter.Eq("_id", postId);
            var update = Builders<Posts>.Update.Inc("Like", 1);
            postsCollection.UpdateOne(filter, update);

        }
        public void AddComment(Comments comment, ObjectId postId)
        {
            var filter = Builders<Posts>.Filter.Eq("_id", postId);
            var update = Builders<Posts>.Update.Push("Comments", comment);
            postsCollection.UpdateOne(filter, update);
        }
        public List<Comments> GetComments(ObjectId postId)
        {
            var filter = Builders<Posts>.Filter.Eq("_id", postId);
            var comment = postsCollection.Find(filter).Project(x => x.comments).First();
            return comment;
        }
        public List<Posts> GetNewPosts(List<string> follows)
        {
            var filter = Builders<Posts>.Filter.In("user", follows);
            var posts = postsCollection.Find(filter).ToList();
            return posts;
        }
        public int GetLike(ObjectId postId)
        {
            var filter = Builders<Posts>.Filter.Eq("_id", postId);
            var like = postsCollection.Find(filter).Project(x => x.Likes).First();
            return like;
        }
        public List<Posts> GetPosts(ObjectId userId) =>
    postsCollection.Find(p => p.user == userId).ToList();

        public Posts GetPost(ObjectId id) =>
          postsCollection.Find(p => p.Id == id).FirstOrDefault();
    }
}
