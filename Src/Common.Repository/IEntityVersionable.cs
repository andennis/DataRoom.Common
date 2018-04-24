using System;

namespace Common.Repository
{
    public interface IEntityVersionable
    {
        int RowVersion { get; set; }
        DateTime CreatedDate { get; set; }
        DateTime UpdatedDate { get; set; }
    }
}