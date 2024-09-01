using ServiceRequestApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceRequestApi.Data
{
    public interface IServiceRequestRepository
    {
        Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync();
        Task<ServiceRequest> GetServiceRequestByIdAsync(Guid id);
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest serviceRequest);
        Task UpdateServiceRequestAsync(ServiceRequest serviceRequest);
        Task DeleteServiceRequestAsync(Guid id);
    }
}
