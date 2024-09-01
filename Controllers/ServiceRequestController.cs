using Microsoft.AspNetCore.Mvc;
using ServiceRequestApi.Data;
using ServiceRequestApi.Models;
using ServiceRequestApi.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ServiceRequestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IServiceRequestRepository _repository;
        private readonly INotificationService _notificationService;

        public ServiceRequestController(IServiceRequestRepository repository, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var requests = await _repository.GetAllServiceRequestsAsync();
            if (requests == null || !requests.Any())
                return NoContent();

            return Ok(requests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var request = await _repository.GetServiceRequestByIdAsync(id);
            if (request == null)
                return NotFound();

            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdRequest = await _repository.CreateServiceRequestAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdRequest.Id }, createdRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingRequest = await _repository.GetServiceRequestByIdAsync(id);
            if (existingRequest == null)
                return NotFound();

            existingRequest.BuildingCode = request.BuildingCode;
            existingRequest.Description = request.Description;
            existingRequest.CurrentStatus = request.CurrentStatus;
            existingRequest.LastModifiedBy = request.LastModifiedBy;
            existingRequest.LastModifiedDate = request.LastModifiedDate;

            await _repository.UpdateServiceRequestAsync(existingRequest);

            // Send notification if the request is closed
            if (existingRequest.CurrentStatus == CurrentStatus.Complete)
            {
                await _notificationService.SendNotificationAsync(existingRequest);
            }

            return Ok(existingRequest);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var request = await _repository.GetServiceRequestByIdAsync(id);
            if (request == null)
                return NotFound();

            await _repository.DeleteServiceRequestAsync(id);
            return NoContent();
        }

    }
}
