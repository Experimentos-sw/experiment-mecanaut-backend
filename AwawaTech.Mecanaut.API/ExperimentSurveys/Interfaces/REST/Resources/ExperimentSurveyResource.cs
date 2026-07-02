namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Resources;

public class ExperimentSurveyResource
{
    public long Id { get; set; }

    public int Rating { get; set; }

    public string Variant { get; set; }

    public string? Action { get; set; }

    public long? UserId { get; set; }

    public DateTime SubmittedAt { get; set; }
}