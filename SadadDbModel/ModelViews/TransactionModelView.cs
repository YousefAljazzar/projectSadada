using SadadDbModel.dbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadadDbModel.ModelViews
{
    public class TransactionModelView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Coantity { get; set; }

        [Timestamp]
        public DateTime CreatedDate { get; set; }
        [Timestamp]

        public DateTime UpdateDate { get; set; }

    }
}
