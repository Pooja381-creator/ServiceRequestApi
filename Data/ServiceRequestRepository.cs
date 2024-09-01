
using Microsoft.EntityFrameworkCore;
using ServiceRequestApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ServiceRequestApi.Data
{
    public class ServiceRequestRepository :IServiceRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public ServiceRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            return await _context.ServiceRequests.ToListAsync();
        }

        public async Task<ServiceRequest> GetServiceRequestByIdAsync(Guid id)
        {
            return await _context.ServiceRequests.FindAsync(id);
        }
        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            serviceRequest.Id = Guid.NewGuid();
            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();
            return serviceRequest;
        }

        public async Task UpdateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            _context.Entry(serviceRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceRequestAsync(Guid id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest != null)
            {
                _context.ServiceRequests.Remove(serviceRequest);
                await _context.SaveChangesAsync();
            }
        }

    }
}
