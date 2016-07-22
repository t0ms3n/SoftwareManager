using System.ComponentModel.DataAnnotations;

namespace SoftwareManager.Entities
{

    public abstract class Entity : IEntity
    {
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

}