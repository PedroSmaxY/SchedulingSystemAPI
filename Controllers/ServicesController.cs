using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchedulingSystemAPI.DTOs.ServiceDtos;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController(IServiceService serviceService) : ControllerBase
    {
        private readonly IServiceService _serviceService = serviceService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll([FromQuery] bool includeInactive = false)
        {
            var services = await _serviceService.GetAllAsync(includeInactive);
            return Ok(services);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServiceDto>> GetById(int id)
        {
            var service = await _serviceService.GetByIdAsync(id);
            if (service == null)
                return NotFound();

            return Ok(service);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceDto>> Create(CreateServiceDto createServiceDto)
        {
            try
            {
                var service = await _serviceService.CreateAsync(createServiceDto);
                return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceDto>> Update(int id, UpdateServiceDto updateServiceDto)
        {
            try
            {
                var service = await _serviceService.UpdateAsync(id, updateServiceDto);
                return Ok(service);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _serviceService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}