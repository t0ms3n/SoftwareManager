namespace SoftwareManager.BLL.Contracts.Models
{

    public class ApplicationApplicationManager : VersionModelBase
    {
        public int ApplicationId { get; set; }

        public Application Application { get; set; }

        public int ApplicationManagerId { get; set; }

        public ApplicationManager ApplicationManager { get; set; }
    }
}