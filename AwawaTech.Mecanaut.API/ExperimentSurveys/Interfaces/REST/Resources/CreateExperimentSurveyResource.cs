namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Resources;

public class CreateExperimentSurveyResource
{
    public int MaintenancePlanId { get; set; }

    public int Rating { get; set; }

    public string Variant { get; set; }

    public string? Action { get; set; }

    public long? UserId { get; set; }

    public string? Comment { get; set; }

    public int? DurationSeconds { get; set; }

    public string? LastStep { get; set; }
}