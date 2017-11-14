using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb;
using UnitOfWorkCore.Samples.MultiContextApi.Models;

namespace UnitOfWorkCore.Samples.MultiContextApi.Services
{
    public class IssuesService : IIssuesService
    {
        private readonly IIssuesUoW _issuesUow;
        private readonly IReleasesUoW _releasesUow;
        private readonly IMapper _mapper;

        public IssuesService(IIssuesUoW issuesUow, 
            IReleasesUoW releasesUow,
            IMapper mapper)
        {
            _issuesUow = issuesUow;
            _releasesUow = releasesUow;
            _mapper = mapper;
        }

        public Issue CreateIssue(CreateIssue data)
        {
            //prepare the initial issue model to insert
            IssueEntity issueEntity = _mapper.Map<IssueEntity>(data);
            ReleaseEntity releaseEntity = null;

            if(data.ExistingPlannedReleaseId.HasValue && data.ExistingPlannedReleaseId.Value != 0)
            {
                //check if the release exists
                releaseEntity = _releasesUow.Releases.SingleOrDefault(x => x.Id == data.ExistingPlannedReleaseId.Value);
                if (releaseEntity == null)
                    throw new Exception("Invalid release id");
                else
                    issueEntity.PlannedReleaseId = data.ExistingPlannedReleaseId;
            }
            else if(data.NewPlannedRelease != null)
            {
                //we can still change to a different isolation level because we haven't done anything with Releases database yet
                _releasesUow.SetIsolationLevel(IsolationLevel.ReadCommitted);

                releaseEntity = _mapper.Map<ReleaseEntity>(data.NewPlannedRelease);
                _releasesUow.Releases.Add(releaseEntity);
                _releasesUow.SaveChanges();

                issueEntity.PlannedReleaseId = releaseEntity.Id;
            }

            //add issue to the database
            _issuesUow.Issues.Add(issueEntity);
            _issuesUow.SaveChanges();

            //prepare and return models
            var issue = _mapper.Map<Issue>(issueEntity);
            if (releaseEntity != null)
                issue.PlannedRelease = _mapper.Map<Release>(releaseEntity);

            return issue;
        }

        public List<Issue> GetAllIssues()
        {
            //get all releases
            var releasesEntities = _releasesUow.Releases.ToList();

            //get all issues and insert release to each model
            var issuesEntities = _issuesUow.Issues.ToList();
            var issues = _mapper.Map<List<Issue>>(issuesEntities);

            foreach(var issue in issues)
            {
                if (issue.PlannedRelease == null)
                    continue;

                var releaseEntity = releasesEntities.SingleOrDefault(x => x.Id == issue.PlannedRelease.Id);
                if (releaseEntity != null)
                    issue.PlannedRelease = _mapper.Map<Release>(releaseEntity);
            }

            return issues;           
        }
    }
}
