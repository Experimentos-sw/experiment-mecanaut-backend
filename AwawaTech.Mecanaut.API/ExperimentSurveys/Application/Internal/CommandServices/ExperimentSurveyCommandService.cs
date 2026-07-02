using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Repositories;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Services;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Application.Internal.CommandServices;

public class ExperimentSurveyCommandService : IExperimentSurveyCommandService
{
    private readonly IExperimentSurveyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ExperimentSurveyCommandService(IExperimentSurveyRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ExperimentSurvey> HandleAsync(CreateExperimentSurveyCommand command)
    {
        var survey = new ExperimentSurvey(
            command.MaintenancePlanId,
            command.Rating,
            command.Variant,
            command.Comment);

        await _repository.AddAsync(survey);
        await _unitOfWork.CompleteAsync();

        return survey;
    }
}