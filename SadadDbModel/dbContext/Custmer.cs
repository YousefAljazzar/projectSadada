using System;
using System.Collections.Generic;

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
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte? IsAracived { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
