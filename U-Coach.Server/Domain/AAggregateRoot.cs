using System;

namespace PVDevelop.UCoach.Server.Domain
{
    /// <summary>
    /// Корневой агрегат
    /// </summary>
    public abstract class AAggregateRoot
    {
        protected AAggregateRoot()
        {
            //Id = Guid.NewGuid();
        }
    }
}
