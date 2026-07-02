using AwawaTech.Mecanaut.API.FormContact.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.FormContact.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.API.FormContact.Interfaces.REST.Transform;

public static class CreateSupportRequestCommandFromResourceAssembler
{
    public static CreateSupportRequestCommand ToCommand(CreateSupportRequestResource resource)
    {
        return new CreateSupportRequestCommand(resource.Name, resource.Email, resource.Message);
    }
}
