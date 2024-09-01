using ServiceRequestApi.Models;
using System.Threading.Tasks;

namespace ServiceRequestApi.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(ServiceRequest serviceRequest);
    }
}
