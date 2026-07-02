using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Commands;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Services;

public interface IExperimentSurveyCommandService
{
    Task<ExperimentSurvey> HandleAsync(CreateExperimentSurveyCommand command);
}
