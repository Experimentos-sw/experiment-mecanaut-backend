using AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Transform;

public static class CreateExperimentSurveyCommandFromResourceAssembler
{
    public static CreateExperimentSurveyCommand ToCommand(CreateExperimentSurveyResource resource)
    {
        return new CreateExperimentSurveyCommand(
            resource.MaintenancePlanId,
            resource.Rating,
            resource.Variant,
            resource.Action,
            resource.UserId,
            resource.Comment,
            resource.DurationSeconds,
            resource.LastStep);
    }
}