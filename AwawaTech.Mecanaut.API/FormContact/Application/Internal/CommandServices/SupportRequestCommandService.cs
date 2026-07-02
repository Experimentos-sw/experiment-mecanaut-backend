using AwawaTech.Mecanaut.API.FormContact.Domain.Model;
using AwawaTech.Mecanaut.API.FormContact.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.FormContact.Domain.Repositories;
using AwawaTech.Mecanaut.API.FormContact.Domain.Services;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;

namespace AwawaTech.Mecanaut.API.FormContact.Application.Internal.CommandServices;

public class SupportRequestCommandService : ISupportRequestCommandService
{
    private readonly ISupportRequestRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SupportRequestCommandService(ISupportRequestRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<SupportRequest> HandleAsync(CreateSupportRequestCommand command)
    {
        var supportRequest = new SupportRequest(command.Name, command.Email, command.Message);
        await _repository.AddAsync(supportRequest);
        await _unitOfWork.CompleteAsync();
        return supportRequest;
    }
}
