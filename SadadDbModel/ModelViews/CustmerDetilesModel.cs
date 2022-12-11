using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadadDbModel.ModelViews
{
    public class CustmerDetilesModel
    {
        public string ProductName { get; set; }

        public double ProductPrice { get; set; }

        [Timestamp]
        public DateTime CreatedDate { get; set; }

    }
}
