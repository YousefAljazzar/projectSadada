using Sadada.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadadDbModel.ModelViews
{
    public class PaginationView
    {
        public PagedResult<CustmerModel> Custmer { get; set; }

        public Dictionary<int, TransactionModelView> Transactions { get; set; }
    }
}
