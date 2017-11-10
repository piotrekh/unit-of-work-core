using System.Collections.Generic;
using UnitOfWorkCore.Samples.SingleContextApi.Models;

namespace UnitOfWorkCore.Samples.SingleContextApi.Services
{
    public interface IReleasesService
    {
        List<Release> GetAllReleases();

        Release CreateRelease(CreateRelease data);
    }
}
