using System;
//using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace Avalentini.Expensi.Core.Data.Entities
{
    public class ExpenseMongoEntity
    {
        [BsonElement("expensemongoentity_id")]
        public string ExpenseMongoEntityId { get; set; }
        //[Column(TypeName = "decimal(18,2)")]
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("what")]
        public string What { get; set; }
        [BsonElement("where")]
        public string Where { get; set; }
        [BsonElement("when")]
        public DateTime When { get; set; }
        [BsonElement("created_at")]
        public DateTime CreationDate { get; set; }

        public override string ToString()
        {
            return $"{nameof(ExpenseMongoEntityId)}: {ExpenseMongoEntityId}, {nameof(Amount)}: {Amount}, {nameof(What)}: {What}, {nameof(Where)}: {Where}, {nameof(When)}: {When}, {nameof(CreationDate)}: {CreationDate}";
        }
    }
}
