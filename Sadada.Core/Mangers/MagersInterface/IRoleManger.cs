using SadadDbModel.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sadada.Core.Mangers.MagersInterface
{
    public interface IRoleManger
    {
        bool CheckAccess(CustmerModel userModel, List<string> persmissions);

    }
}
