using System;
using System.Collections.Generic;

#nullable disable

namespace SadadDbModel.dbContext
{
    public partial class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Coantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Custmer User { get; set; }
    }
}
