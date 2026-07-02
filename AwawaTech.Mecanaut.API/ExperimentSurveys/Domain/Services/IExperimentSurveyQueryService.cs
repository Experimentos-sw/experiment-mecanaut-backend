using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Services;

public interface IExperimentSurveyQueryService
{
    Task<IEnumerable<ExperimentSurvey>> ListAsync();

    Task<ExperimentSurvey?> FindByIdAsync(long id);
}