namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Commands;

public record CreateExperimentSurveyCommand(
    Guid MaintenancePlanId,
    int Rating,
    string Variant,
    string? Comment
);