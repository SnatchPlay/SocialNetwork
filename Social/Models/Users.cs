using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Social.Models
{
    public class Users
    {
        [BsonElement("FirstName")]
        public string Name { get; set; }
        [BsonElement("LastName")]
        public string LastName { get; set; }
        [BsonElement("Email")]
        public string Email { get; set; }
        [BsonElement("Password")]
        public string Password { get; set; }
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("Folows")]
        public List<string> Follows { get; set; }
    }
}
