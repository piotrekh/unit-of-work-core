using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitOfWorkCore.Samples.SingleContextApi.DataAccess;
using UnitOfWorkCore.Samples.SingleContextApi.Models;

namespace UnitOfWorkCore.Samples.SingleContextApi.Services
{
    public class ReleasesService : IReleasesService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ReleasesService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public Release CreateRelease(CreateRelease data)
        {
            var entity = _mapper.Map<ReleaseEntity>(data);
            _uow.Releases().Add(entity);

            //save changes to obtain the entity id
            _uow.SaveChanges();
            return _mapper.Map<Release>(entity);            
        }

        public List<Release> GetAllReleases()
        {
            var entities = _uow.Releases().ToList();
            var releases = _mapper.Map<List<Release>>(entities);
            return releases;
        }
    }
}
