using AutoMapper;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;

namespace Sadada.Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Custmer, RegisterCompleteView>().ReverseMap();

        }
    }
}
