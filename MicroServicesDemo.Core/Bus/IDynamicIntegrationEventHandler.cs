using System.Threading.Tasks;

namespace MicroServicesDemo.Bus
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}
