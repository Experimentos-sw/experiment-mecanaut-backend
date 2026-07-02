using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Repositories;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using AwawaTech.Mecanaut.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Infrastructure.Persistence.EFC.Repositories;

public class ExperimentSurveyRepository : BaseRepository<ExperimentSurvey>, IExperimentSurveyRepository
{
    public ExperimentSurveyRepository(AppDbContext context) : base(context)
    {
    }
}