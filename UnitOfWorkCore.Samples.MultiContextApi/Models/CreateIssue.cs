namespace UnitOfWorkCore.Samples.MultiContextApi.Models
{
    public class CreateIssue
    {
        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Allows to choose the existing release
        /// </summary>
        public int? ExistingPlannedReleaseId { get; set; }

        /// <summary>
        /// Allows to create a new release along with the issue
        /// </summary>
        public CreateRelease NewPlannedRelease { get; set; }
    }
}
