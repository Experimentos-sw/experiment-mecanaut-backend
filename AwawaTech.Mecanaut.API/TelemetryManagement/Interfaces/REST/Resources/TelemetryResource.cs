namespace AwawaTech.Mecanaut.API.TelemetryManagement.Interfaces.REST.Resources
{
    public record TelemetryResource(
        string ExperimentName,
        string Variant,
        string ActionType,
        long? DurationMilliseconds,
        bool IsSuccess,
        string AdditionalData
    );
}
