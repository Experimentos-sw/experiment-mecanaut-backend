using AwawaTech.Mecanaut.API.FormContact.Domain.Model;
using AwawaTech.Mecanaut.API.FormContact.Domain.Repositories;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace AwawaTech.Mecanaut.API.FormContact.Infrastructure.Persistence.EFC.Repositories;

public class SupportRequestRepository : BaseRepository<SupportRequest>, ISupportRequestRepository
{
    public SupportRequestRepository(AppDbContext context) : base(context)
    {
    }
}
