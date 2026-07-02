using AwawaTech.Mecanaut.API.IAM.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.IAM.Interfaces.REST.Resources;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(
            resource.Ruc,
            resource.LegalName,
            resource.CommercialName,
            resource.Address,
            resource.City,
            resource.Country,
            string.IsNullOrWhiteSpace(resource.TenantPhone) ? null : new PhoneNumber(resource.TenantPhone),
            new EmailAddress(resource.TenantEmail),
            resource.Website,
            resource.SubscriptionPlanId,
            resource.Username,
            resource.Password,
            new EmailAddress(resource.Email),
            resource.FirstName,
            resource.LastName,
            resource.Role);
    }
}