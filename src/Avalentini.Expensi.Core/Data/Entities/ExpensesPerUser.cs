using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Avalentini.Expensi.Core.Data.Entities
{
    public class ExpensesPerUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ObjectId { get; set; }

        [BsonElement("user_id")]
        public int UserId { get; set; }

        [BsonElement("expenses")]
        public List<ExpenseMongoEntity> Expenses { get; set; } = new List<ExpenseMongoEntity>();
    }
}