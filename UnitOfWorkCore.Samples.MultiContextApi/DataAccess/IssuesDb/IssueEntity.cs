namespace UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb
{
    public class IssueEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? PlannedReleaseId { get; set; }
    }
}
