using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avalentini.Expensi.Core.Data.Entities
{
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        [BsonElement("user_id")]
        public int UserId { get; set; }
        
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreationDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactEmail { get; set; }
        
        [BsonElement("expenses")]
        public List<ExpenseMongoEntity> Expenses { get; set; } = new List<ExpenseMongoEntity>();
    }
}
