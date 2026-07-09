using System;
using AwawaTech.Mecanaut.API.Shared.Domain.Model.Entities;

namespace AwawaTech.Mecanaut.API.TelemetryManagement.Domain.Model.Aggregates
{
    public class ExperimentLog : AuditableEntity
    {
        public int Id { get; private set; }
        public string ExperimentName { get; private set; } // Ej: "EC-01", "EC-03", "Auditoria-6.4.2"
        public string Variant { get; private set; }        // "Control" (Back Antiguo) o "Treatment" (Back Nuevo)
        public string ActionType { get; private set; }     // Ej: "Template_Created", "Template_Used", "Landing_Form_Submitted", "Order_Start_Attempt"
        public long? DurationMilliseconds { get; private set; } // Tiempo que tomó la acción (opcional)
        public bool IsSuccess { get; private set; }        // Si la operación terminó con éxito o fue bloqueada/errónea
        public string AdditionalData { get; private set; } // JSON o string corto con datos extra (ej: repuestos faltantes, score de encuesta, rol seleccionado)

        // Constructor requerido por Entity Framework
        protected ExperimentLog() {}

        public ExperimentLog(string experimentName, string variant, string actionType, long? durationMilliseconds, bool isSuccess, string additionalData)
        {
            ExperimentName = experimentName;
            Variant = variant;
            ActionType = actionType;
            DurationMilliseconds = durationMilliseconds;
            IsSuccess = isSuccess;
            AdditionalData = additionalData;
        }
    }
}
