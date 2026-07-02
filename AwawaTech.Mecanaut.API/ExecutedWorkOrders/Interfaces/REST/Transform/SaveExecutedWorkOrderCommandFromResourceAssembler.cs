using System.Linq;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.ExecutedWorkOrders.Interfaces.REST.Resources;

namespace AwawaTech.Mecanaut.API.ExecutedWorkOrders.Interfaces.REST.Transform;

public class SaveExecutedWorkOrderCommandFromResourceAssembler
{
    public static CreateExecutedWorkOrderCommand ToCommand(SaveExecutedWorkOrderResource resource)
    {
        return new CreateExecutedWorkOrderCommand
        {
            Code = resource.Code,
            Annotations = resource.Annotations,
            ExecutionDate = resource.ExecutionDate,
            ProductionLineId = resource.ProductionLineId,
            IntervenedMachineIds = resource.IntervenedMachineIds,
            AssignedTechnicianIds = resource.AssignedTechnicianIds,
            ExecutedTasks = resource.ExecutedTasks,
            UsedProducts = resource.UsedProducts
                .Select(p => (p.ProductId, p.Quantity))
                .ToList()
        };
    }
} 