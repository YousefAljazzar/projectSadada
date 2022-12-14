using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SadadDbModel.ModelViews
{
    public class UserLoginView
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
