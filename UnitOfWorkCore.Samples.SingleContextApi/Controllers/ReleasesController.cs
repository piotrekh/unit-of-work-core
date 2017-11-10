using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using UnitOfWorkCore.AspNetCore.Attributes;
using UnitOfWorkCore.Samples.SingleContextApi.Models;
using UnitOfWorkCore.Samples.SingleContextApi.Services;

namespace UnitOfWorkCore.Samples.SingleContextApi.Controllers
{
    [Route("api/[controller]")]
    public class ReleasesController : ControllerBase
    {
        private readonly IReleasesService _releasesService;

        public ReleasesController(IReleasesService releasesService)
        {
            _releasesService = releasesService;
        }

        /// <summary>
        /// Returns all releases
        /// </summary>
        /// <remarks>
        /// This method should not generate any transaction
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(List<Release>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllReleases()
        {
            var releases = _releasesService.GetAllReleases();
            return Ok(releases);
        }

        /// <summary>
        /// Creates a release, without specifying the transaction isolation level
        /// </summary>
        /// <remarks>
        /// When creating a transaction, isolation level is not specified,
        /// which means that the database default transaction isolation level
        /// should be used (READ COMMITTED for Sql Server)
        /// </remarks>
        [HttpPost("unspecifiedisolationlevel")]
        [ProducesResponseType(typeof(Release), (int)HttpStatusCode.OK)]
        public IActionResult CreateReleaseWithDefaultTransaction([FromBody] CreateRelease data)
        {
            var release = _releasesService.CreateRelease(data);
            return Ok(release);
        }

        /// <summary>
        /// Creates a release, using SERIALIZABLE transaction isolation level
        /// </summary>
        /// <remarks>
        /// When creating a transaction, uses SERIALIZABLE isolation level
        /// </remarks>
        [HttpPost("serializableisolationlevel")]
        [ProducesResponseType(typeof(Release), (int)HttpStatusCode.OK)]
        [TransactionIsolationLevel(System.Data.IsolationLevel.Serializable)]
        public IActionResult CreateReleaseWithSerializableTransaction([FromBody] CreateRelease data)
        {
            var release = _releasesService.CreateRelease(data);
            return Ok(release);
        }
    }
}
