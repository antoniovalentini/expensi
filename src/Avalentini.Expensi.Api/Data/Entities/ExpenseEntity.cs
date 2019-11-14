using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avalentini.Expensi.Api.Data.Entities
{
    public class ExpenseEntity
    {
        public string Id { get; set; }
        public UserEntity User { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime When { get; set; }
        public string Where { get; set; }
        public string What { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class UserEntity
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CreationDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactEmail { get; set; }
    }
}
