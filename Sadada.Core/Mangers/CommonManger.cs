using AutoMapper;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sadada.Common.Extensions;

namespace Sadada.Core.Mangers
{
    public class CommonManger :ICommonManger
    {
        private sadaddbContext _sadaddbContext;
        private IMapper _mapper;

        public CommonManger(sadaddbContext sadaddbContext, IMapper mapper)
        {
            _sadaddbContext = sadaddbContext;
            _mapper = mapper;
        }

        public CustmerModel GetUserRole(CustmerModel custmer)
        {
            var dbUser = _sadaddbContext.Custmers
                                      .FirstOrDefault(a => a.Email == custmer.Email)
                                      ?? throw new ServiceValidationException("Invalid user id received");

            var mappedUser = _mapper.Map<CustmerModel>(dbUser);

            return mappedUser;
        }
    }
}
