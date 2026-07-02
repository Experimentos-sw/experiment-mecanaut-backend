namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Resources;

public class CreateExperimentSurveyResource
{
    public int MaintenancePlanId { get; set; }

    public int Rating { get; set; }

    public string Variant { get; set; }

    public string? Comment { get; set; }
}