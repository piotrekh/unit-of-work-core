using AutoMapper;
using UnitOfWorkCore.Samples.SingleContextApi.DataAccess;
using UnitOfWorkCore.Samples.SingleContextApi.Models;

namespace UnitOfWorkCore.Samples.SingleContextApi
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ReleaseEntity, Release>();

            CreateMap<Release, ReleaseEntity>();

            CreateMap<CreateRelease, ReleaseEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
