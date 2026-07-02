namespace AwawaTech.Mecanaut.API.FormContact.Domain.Model.Commands;

public record CreateSupportRequestCommand(string Name, string Email, string Message);
