using JPProject.Domain.Core.Events;
using Microsoft.EntityFrameworkCore;

namespace JPProject.EntityFrameworkCore.Interfaces
{
    public interface IEventStoreContext : IJpEntityFrameworkStore
    {
        public DbSet<StoredEvent> StoredEvent { get; set; }
        public DbSet<EventDetails> StoredEventDetails { get; set; }
    }
}
