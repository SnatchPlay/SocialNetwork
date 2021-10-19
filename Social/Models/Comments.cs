using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Social.Models
{
    public class Comments
    {
        [BsonElement("likes")]
        public int Likes { get; set; }
        [BsonElement("body")]
        public string Body { get; set; }
        [BsonElement("user")]
        public ObjectId User { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }
    }
}
