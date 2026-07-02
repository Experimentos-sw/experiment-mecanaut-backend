namespace AwawaTech.Mecanaut.API.FormContact.Interfaces.REST.Resources;

public class CreateSupportRequestResource
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Message { get; set; } = null!;
}
