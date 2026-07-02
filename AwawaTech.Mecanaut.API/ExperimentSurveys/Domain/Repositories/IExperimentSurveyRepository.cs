using AwawaTech.Mecanaut.API.Shared.Domain.Repositories;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Repositories;

using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;

public interface IExperimentSurveyRepository : IBaseRepository<ExperimentSurvey>
{
    Task AddAsync(ExperimentSurvey survey);

    Task<IEnumerable<ExperimentSurvey>> ListAsync();
}