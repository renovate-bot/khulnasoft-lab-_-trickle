using Trickle.Directory.Api.Contracts.Models;
using Refit;

namespace Trickle.Directory.Api.Contracts;

public interface IDirectoryApi
{
    [Get("/lists")]
    Task<IEnumerable<ListVm>> GetListsAsync(CancellationToken cancellationToken);

    [Get("/lists/{id}")]
    Task<ListDetailsVm> GetListDetailsAsync(long id, CancellationToken cancellationToken);
}
