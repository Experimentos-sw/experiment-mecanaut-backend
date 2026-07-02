using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Repositories;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Services;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Application.Internal.QueryServices;

public class ExperimentSurveyQueryService : IExperimentSurveyQueryService
{
    private readonly IExperimentSurveyRepository _repository;

    public ExperimentSurveyQueryService(IExperimentSurveyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ExperimentSurvey>> ListAsync()
    {
        return await _repository.ListAsync();
    }
    
    public async Task<ExperimentSurvey?> FindByIdAsync(long id)
    {
        return await _repository.FindByIdAsync(id);
    }
}
