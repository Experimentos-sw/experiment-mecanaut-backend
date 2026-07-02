using AwawaTech.Mecanaut.API.FormContact.Domain.Model;
using AwawaTech.Mecanaut.API.FormContact.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.API.FormContact.Interfaces.REST.Transform;

public static class SupportRequestToResourceAssembler
{
    public static SupportRequestResource ToResource(SupportRequest supportRequest)
    {
        return new SupportRequestResource
        {
            Id = supportRequest.Id,
            Name = supportRequest.Name,
            Email = supportRequest.Email,
            Message = supportRequest.Message,
            SubmittedAt = supportRequest.SubmittedAt
        };
    }
}
