namespace AwawaTech.Mecanaut.API.FormContact.Domain.Model;

using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;

public class SupportRequest : AuditableAggregateRoot
{
    public string Name { get; private set; }

    public string Email { get; private set; }

    public string Message { get; private set; }

    public DateTime SubmittedAt { get; private set; }

    public SupportRequest(string name, string email, string message)
    {
        Name = name;
        Email = email;
        Message = message;
        SubmittedAt = DateTime.UtcNow;
    }
}
