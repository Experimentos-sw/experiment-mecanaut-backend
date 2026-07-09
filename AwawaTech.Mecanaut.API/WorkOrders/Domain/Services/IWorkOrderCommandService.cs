using System.Threading.Tasks;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Aggregates;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.Commands;
using AwawaTech.Mecanaut.API.WorkOrders.Domain.Model.ValueObjects;

namespace AwawaTech.Mecanaut.API.WorkOrders.Domain.Services;

public interface IWorkOrderCommandService
{
    Task<WorkOrder> Handle(CreateWorkOrderCommand command);
    Task<WorkOrder> Handle(CompleteWorkOrderCommand command);
    Task<WorkOrder> Handle(AssignTechniciansCommand command);
    Task<StartWorkOrderResult> Handle(StartWorkOrderCommand command);
} 