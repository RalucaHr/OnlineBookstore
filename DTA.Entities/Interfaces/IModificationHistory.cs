using System;

namespace DTA.Entities
{
    public interface IModificationHistory
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        bool IsDirty { get; set; }
    }
}
