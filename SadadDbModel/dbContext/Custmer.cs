using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SadadDbModel.dbContext
{
    public partial class Custmer
    {
        public Custmer()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAracived { get; set; }

        public double TotalDept { get; set; }

        [Timestamp]
        public DateTime CreatedDate { get; set; }
        [Timestamp]
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
