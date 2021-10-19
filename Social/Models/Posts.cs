using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Social.Models
{
    public class Posts
    {
        [BsonElement("body")]
        public string Body { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("likes")]
        public int Likes { get; set; }
        [BsonIgnoreIfNull]
        public ObjectId user { get; set; }
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("comments")]
        public List<Comments> comments { get; set; }


    }
}
