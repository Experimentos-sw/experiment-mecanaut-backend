using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Transform;

public static class ExperimentSurveyToResourceAssembler
{
    public static ExperimentSurveyResource ToResource(ExperimentSurvey survey)
    {
        return new ExperimentSurveyResource
        {
            Id = survey.Id,
            Rating = survey.Rating,
            Variant = survey.Variant,
            Action = survey.Action,
            UserId = survey.UserId,
            SubmittedAt = survey.SubmittedAt
        };
    }
}