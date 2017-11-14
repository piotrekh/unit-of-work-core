using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnitOfWorkCore.Samples.MultiContextApi.Services;
using UnitOfWorkCore.Samples.MultiContextApi.Models;
using System.Net;
using UnitOfWorkCore.AspNetCore.Attributes;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb;
using System.Data;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb;

namespace UnitOfWorkCore.Samples.MultiContextApi.Controllers
{
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly IIssuesService _issuesService;

        public IssuesController(IIssuesService issuesService)
        {
            _issuesService = issuesService;
        }

        /// <summary>
        /// Returns all issues
        /// </summary>
        /// <remarks>
        /// This method should not generate any transaction
        /// </remarks>
        [HttpGet]
        [ProducesResponseType(typeof(List<Issue>), (int)HttpStatusCode.OK)]
        public IActionResult GetAllIssues()
        {
            var issues = _issuesService.GetAllIssues();
            return Ok(issues);
        }

        /// <summary>
        /// Creates an issue
        /// </summary>
        /// <remarks>
        /// Uses READ UNCOMMITTED transaction isolation level for calls to Releases database
        /// and SERIALIZABLE for calls to Issues database
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(Issue), (int)HttpStatusCode.OK)]
        [TransactionIsolationLevel(IsolationLevel.ReadUncommitted, UnitOfWorkKey = ReleasesUoW.KEY)]
        [TransactionIsolationLevel(IsolationLevel.Serializable, UnitOfWorkKey = IssuesUow.KEY)]
        public IActionResult CreateIssue([FromBody] CreateIssue data)
        {
            var issue = _issuesService.CreateIssue(data);
            return Ok(issue);
        }
    }
}
