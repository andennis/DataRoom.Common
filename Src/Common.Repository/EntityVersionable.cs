using System;

namespace Common.Repository
{
    public abstract class EntityVersionable : IEntityVersionable
    {
        public int RowVersion { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
