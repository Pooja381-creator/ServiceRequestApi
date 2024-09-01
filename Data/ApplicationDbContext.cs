using Microsoft.EntityFrameworkCore;
using ServiceRequestApi.Models;
namespace ServiceRequestApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}
