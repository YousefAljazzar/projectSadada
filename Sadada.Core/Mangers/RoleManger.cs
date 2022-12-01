using AutoMapper;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Common.Extensions;

namespace Sadada.Core.Mangers
{
    public class RoleManger:IRoleManger
    {
        private sadaddbContext _sadaddbContext;
        private IMapper _mapper;

        public RoleManger(sadaddbContext sadaddbContext, IMapper mapper)
        {
            _sadaddbContext = sadaddbContext;
            _mapper = mapper;
        }



        public bool CheckAccess(CustmerModel userModel, List<string> persmissions)
        {
            var userTest = _sadaddbContext.Custmers
                                        .FirstOrDefault(a => a.Id == userModel.Id)
                                        ?? throw new ServiceValidationException("Invalid user id");

           /* if (userTest.IsSuperAdmin)
            {
                return true;
            }*/


            return false;
        }

    }
}
