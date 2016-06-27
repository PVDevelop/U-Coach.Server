using System;

namespace PVDevelop.UCoach.Server.Domain
{
    /// <summary>
    /// Корневой агрегат
    /// </summary>
    public abstract class AAggregateRoot
    {
        /// <summary>
        /// Идентификатор рута
        /// </summary>
        public Guid Id { get; private set; }

        protected AAggregateRoot()
        {
            Id = Guid.NewGuid();
        }
    }
}
