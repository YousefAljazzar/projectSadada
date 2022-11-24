using AutoMapper;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sadada.Core.Mangers
{
    public class CustmerManger : ICustmerManger
    {
        private readonly sadaddbContext _sadaddbContext;
        private IMapper _mapper;
        public void CreateCustmer(CreateCustmerView custmer)
        {
            
        }
    }
}
