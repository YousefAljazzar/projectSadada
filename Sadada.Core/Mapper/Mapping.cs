using AutoMapper;
using Sadada.Common.Extensions;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;

namespace Sadada.Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Custmer, RegisterCompleteView>().ReverseMap();
            CreateMap<Custmer,LoginReponseView>().ReverseMap();    
            CreateMap<Custmer,CustmerModel>().ReverseMap();    
            CreateMap<Custmer,ForgetCustmerView>().ReverseMap();    
            CreateMap<PagedResult<CustmerModel>, PagedResult<Custmer>>().ReverseMap();    
            CreateMap<TransactionModelView, Transaction>().ReverseMap();    

        }
    }
}
