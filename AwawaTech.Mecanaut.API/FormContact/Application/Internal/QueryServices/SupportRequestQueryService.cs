using AwawaTech.Mecanaut.API.FormContact.Domain.Model;
using AwawaTech.Mecanaut.API.FormContact.Domain.Repositories;
using AwawaTech.Mecanaut.API.FormContact.Domain.Services;

namespace AwawaTech.Mecanaut.API.FormContact.Application.Internal.QueryServices;

public class SupportRequestQueryService : ISupportRequestQueryService
{
    private readonly ISupportRequestRepository _repository;

    public SupportRequestQueryService(ISupportRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<SupportRequest?> FindByIdAsync(long id)
    {
        return await _repository.FindByIdAsync(id);
    }

    public async Task<IEnumerable<SupportRequest>> ListAsync()
    {
        return await _repository.ListAsync();
    }
}
