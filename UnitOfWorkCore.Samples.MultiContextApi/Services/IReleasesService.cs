using System.Collections.Generic;
using UnitOfWorkCore.Samples.MultiContextApi.Models;

namespace UnitOfWorkCore.Samples.MultiContextApi.Services
{
    public interface IReleasesService
    {
        List<Release> GetAllReleases();
    }
}
