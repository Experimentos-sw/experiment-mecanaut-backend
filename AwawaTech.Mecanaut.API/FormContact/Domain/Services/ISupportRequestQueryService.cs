using AwawaTech.Mecanaut.API.FormContact.Domain.Model;

namespace AwawaTech.Mecanaut.API.FormContact.Domain.Services;

public interface ISupportRequestQueryService
{
    Task<SupportRequest?> FindByIdAsync(long id);
    Task<IEnumerable<SupportRequest>> ListAsync();
}
