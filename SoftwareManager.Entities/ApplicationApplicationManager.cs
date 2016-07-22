namespace SoftwareManager.Entities
{

    public class ApplicationApplicationManager : DateTrackedEntity
    {
        public int ApplicationId { get; set; }
        public Application Application { get; set; }

        public int ApplicationManagerId { get; set; }
        public ApplicationManager ApplicationManager { get; set; }

    }

}