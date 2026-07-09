namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Commands;

public record CreateExperimentSurveyCommand
{
    public int MaintenancePlanId { get; init; }
    public int Rating { get; init; }
    public string Variant { get; init; }
    public string? Action { get; init; }
    public long? UserId { get; init; }
    public string? Comment { get; init; }
    public int? DurationSeconds { get; init; }
    public string? LastStep { get; init; }

    public CreateExperimentSurveyCommand()
    {
        Variant = string.Empty;
    }

    public CreateExperimentSurveyCommand(int maintenancePlanId, int rating, string variant, string? action, long? userId, string? comment, int? durationSeconds, string? lastStep)
    {
        MaintenancePlanId = maintenancePlanId;
        Rating = rating;
        Variant = variant;
        Action = action;
        UserId = userId;
        Comment = comment;
        DurationSeconds = durationSeconds;
        LastStep = lastStep;
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