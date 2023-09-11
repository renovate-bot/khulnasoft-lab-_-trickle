using Trickle.SharedKernel.Domain.SeedWork;

namespace Trickle.Archival.Domain.ListArchives;

public interface IListArchiveRepository : IUnitOfWork
{
    Task AddAsync(ListArchive listArchive, CancellationToken cancellationToken);
}
