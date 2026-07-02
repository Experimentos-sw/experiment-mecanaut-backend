namespace AwawaTech.Mecanaut.API.WorkOrders.Interfaces.REST.Resources;

public class CompleteWorkOrderResource
{
    public bool IsAreaCleaned { get; set; }
    public bool AreToolsReturned { get; set; }
    public bool IsOperationsVerified { get; set; }
}
