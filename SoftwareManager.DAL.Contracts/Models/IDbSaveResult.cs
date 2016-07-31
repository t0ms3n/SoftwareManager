namespace SoftwareManager.DAL.Contracts.Models
{
    public interface IDbSaveResult
    {
        int AffectedRows { get; set; }
    }
    
}