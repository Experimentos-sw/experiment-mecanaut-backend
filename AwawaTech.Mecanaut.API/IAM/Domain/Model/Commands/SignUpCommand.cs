using AwawaTech.Mecanaut.API.Shared.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.IAM.Domain.Model.Commands;

/**
 * <summary>
 *     The sign up command
 * </summary>
 * <remarks>
 *     This command object includes the username and password to sign up
 * </remarks>
 */
public record SignUpCommand(
    // Tenant data
    string Ruc,
    string LegalName,
    string? CommercialName,
    string? Address,
    string? City,
    string? Country,
    PhoneNumber? TenantPhone,
    EmailAddress TenantEmail,
    string? Website,
    long SubscriptionPlanId,

    // User data
    string Username,
    string Password,
    EmailAddress Email,
    string FirstName,
    string LastName,
    string? Role = null
);