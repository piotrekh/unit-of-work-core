namespace UnitOfWorkCore.Samples.MultiContextApi.Models
{
    public class Issue
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Release PlannedRelease { get; set; }
    }
}
