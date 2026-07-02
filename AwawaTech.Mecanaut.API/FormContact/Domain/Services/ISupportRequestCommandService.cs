using AwawaTech.Mecanaut.API.FormContact.Domain.Model;
using AwawaTech.Mecanaut.API.FormContact.Domain.Model.Commands;

namespace AwawaTech.Mecanaut.API.FormContact.Domain.Services;

public interface ISupportRequestCommandService
{
    Task<SupportRequest> HandleAsync(CreateSupportRequestCommand command);
}
