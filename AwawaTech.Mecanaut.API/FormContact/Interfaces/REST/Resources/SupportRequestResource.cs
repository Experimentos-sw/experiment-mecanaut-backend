namespace AwawaTech.Mecanaut.API.FormContact.Interfaces.REST.Resources;

public class SupportRequestResource
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
    public DateTime SubmittedAt { get; set; }
}
