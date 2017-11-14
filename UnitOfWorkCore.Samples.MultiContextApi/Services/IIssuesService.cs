using System.Collections.Generic;
using UnitOfWorkCore.Samples.MultiContextApi.Models;

namespace UnitOfWorkCore.Samples.MultiContextApi.Services
{
    public interface IIssuesService
    {
        List<Issue> GetAllIssues();

        Issue CreateIssue(CreateIssue data);
    }
}
