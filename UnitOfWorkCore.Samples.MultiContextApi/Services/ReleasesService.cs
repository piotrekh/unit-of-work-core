using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb;
using UnitOfWorkCore.Samples.MultiContextApi.Models;

namespace UnitOfWorkCore.Samples.MultiContextApi.Services
{
    public class ReleasesService : IReleasesService
    {
        private readonly IReleasesUoW _uow;
        private readonly IMapper _mapper;

        public ReleasesService(IReleasesUoW uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public List<Release> GetAllReleases()
        {
            var entities = _uow.Releases.ToList();
            var releases = _mapper.Map<List<Release>>(entities);
            return releases;
        }
    }
}
