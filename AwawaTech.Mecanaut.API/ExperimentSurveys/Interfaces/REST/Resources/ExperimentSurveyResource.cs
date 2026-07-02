namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Interfaces.REST.Resources;

public class ExperimentSurveyResource
{
    public int Id { get; set; }

    public int Rating { get; set; }

    public string Variant { get; set; }

    public DateTime SubmittedAt { get; set; }
}