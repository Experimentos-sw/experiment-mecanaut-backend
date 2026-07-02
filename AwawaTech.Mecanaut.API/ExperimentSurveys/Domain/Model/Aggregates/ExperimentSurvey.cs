namespace AwawaTech.Mecanaut.API.ExperimentSurveys.Domain.Model.Aggregates;

using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;

public class ExperimentSurvey : AuditableAggregateRoot
{
    public long Id { get; private set; }

    public int MaintenancePlanId { get; private set; }

    public int Rating { get; private set; }

    public string Variant { get; private set; }

    public string? Action { get; private set; }

    public long? UserId { get; private set; }

    public string? Comment { get; private set; }

    public DateTime SubmittedAt { get; private set; }

    public ExperimentSurvey(
        int maintenancePlanId,
        int rating,
        string variant,
        string? action,
        long? userId,
        string? comment)
    {
        MaintenancePlanId = maintenancePlanId;
        Rating = rating;
        Variant = variant;
        Action = action;
        UserId = userId;
        Comment = comment;
        SubmittedAt = DateTime.UtcNow;
    }
}