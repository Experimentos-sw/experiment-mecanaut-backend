using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;
using AwawaTech.Mecanaut.API.FormContact.Domain.Model;

namespace AwawaTech.Mecanaut.API.FormContact.Domain.Repositories;

public interface ISupportRequestRepository : IBaseRepository<SupportRequest>
{
    Task AddAsync(SupportRequest supportRequest);
}
