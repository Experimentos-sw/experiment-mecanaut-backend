using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Repositories;
using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Application.Internal.CommandServices;

public class ExperimentSurveyCommandService
{
    private readonly IExperimentSurveyRepository repository;
    private readonly IUnitOfWork unitOfWork;

    public async Task Handle(CreateExperimentSurveyCommand command)
    {
        var survey = new ExperimentSurvey(
            command.MaintenancePlanId,
            command.Rating,
            command.Variant,
            command.Comment);

        await repository.AddAsync(survey);

        await unitOfWork.CompleteAsync();
    }
}