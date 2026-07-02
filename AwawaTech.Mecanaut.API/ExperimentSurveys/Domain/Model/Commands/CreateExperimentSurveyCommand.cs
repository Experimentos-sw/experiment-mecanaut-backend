namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Commands;

public record CreateExperimentSurveyCommand
{
    public Guid MaintenancePlanId { get; init; }
    public int Rating { get; init; }
    public string Variant { get; init; }
    public string? Comment { get; init; }

    public CreateExperimentSurveyCommand()
    {
        Variant = string.Empty;
    }

    public CreateExperimentSurveyCommand(string? maintenancePlanId, int rating, string variant, string? comment)
    {
        MaintenancePlanId = ParseMaintenancePlanId(maintenancePlanId);
        Rating = rating;
        Variant = variant;
        Comment = comment;
    }

    private static Guid ParseMaintenancePlanId(string? maintenancePlanId)
    {
        if (string.IsNullOrWhiteSpace(maintenancePlanId))
        {
            throw new ArgumentException("maintenancePlanId is required.", nameof(maintenancePlanId));
        }

        if (Guid.TryParse(maintenancePlanId, out var guid))
        {
            return guid;
        }

        if (long.TryParse(maintenancePlanId, out var numericId))
        {
            return Guid.Parse($"00000000-0000-0000-0000-{numericId:D12}");
        }

        throw new ArgumentException($"Invalid maintenancePlanId '{maintenancePlanId}'. Expected a GUID or numeric value.", nameof(maintenancePlanId));
    }
}