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
    public class RepositoryUser
    {
        static string connString = "mongodb://localhost:27017";
        static MongoClient client = new MongoClient(connString);
        IMongoDatabase database = client.GetDatabase("sc");
        IMongoCollection<Users> userCollection;
        public RepositoryUser()
        {
            ReadFromMongo();
        }
        public void ReadFromMongo()
        {
            userCollection = database.GetCollection<Users>("users");
        }
        public void Add(Users user) =>
           userCollection.InsertOne(user);
        public void ChangePassword(string password, Users user) =>
            userCollection.ReplaceOne(entity => entity.Password == password, user);
        public void AddFollowing(string email, string newFollowing)
        {
            UpdateDefinition<Users> update;
            var filter = Builders<Users>.Filter.Eq("Email", email);
            if (userCollection.Find(entity => entity.Email == email).FirstOrDefault().Follows == null)
            {
                update = Builders<Users>.Update.Set("Folows", newFollowing);
            }
            else
            {
                update = Builders<Users>.Update.Push("Folows", newFollowing);
            }
            userCollection.UpdateOne(filter, update);

        }
        public List<Users> GetUsers() =>
             userCollection.Find(entity => true).ToList();

        public Users GetUser(string email) =>
           userCollection.Find(entity => entity.Email == email).FirstOrDefault();
        public ObjectId GetUserId(string email)
        {
            var user = userCollection.Find(entity => entity.Email == email).FirstOrDefault();
            return user.Id;
        }
    }
}
