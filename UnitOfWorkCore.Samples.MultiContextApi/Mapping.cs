using AutoMapper;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb;
using UnitOfWorkCore.Samples.MultiContextApi.Models;

namespace UnitOfWorkCore.Samples.MultiContextApi
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ReleaseEntity, Release>();

            CreateMap<IssueEntity, Issue>()
                //a trick to pass the PlannedReleaseId into nested model (other properties
                //are not set, but the whole Release model will be overwritten anyway)
                .ForMember(dest => dest.PlannedRelease, opt => opt.MapFrom(src => 
                    src.PlannedReleaseId.HasValue ? new Release() { Id = src.PlannedReleaseId.Value } : null));

            CreateMap<CreateIssue, IssueEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PlannedReleaseId, opt => opt.Ignore());

            CreateMap<CreateRelease, ReleaseEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
