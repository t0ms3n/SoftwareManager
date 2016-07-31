namespace SoftwareManager.DAL.Contracts.Models
{

    public abstract class Entity : IEntity
    {
        public int Id { get; set; }

        public byte[] RowVersion { get; set; }
    }

}