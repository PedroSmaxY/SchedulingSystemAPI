using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchedulingSystemAPI.DTOs.AvailableSlotDtos;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailableSlotsController : ControllerBase
    {
        private readonly IAvailableSlotService _availableSlotService;

        public AvailableSlotsController(IAvailableSlotService availableSlotService)
        {
            _availableSlotService = availableSlotService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvailableSlotDto>>> GetAll()
        {
            var slots = await _availableSlotService.GetAllAsync();
            return Ok(slots);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AvailableSlotDto>> GetById(int id)
        {
            var slot = await _availableSlotService.GetByIdAsync(id);
            if (slot == null)
                return NotFound();

            return Ok(slot);
        }

        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<AvailableSlotDto>>> GetAvailable()
        {
            var slots = await _availableSlotService.GetAvailableSlotsAsync();
            return Ok(slots);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<AvailableSlotDto>>> GetByDate(DateTime date)
        {
            var slots = await _availableSlotService.GetByDateAsync(date);
            return Ok(slots);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AvailableSlotDto>> Create(CreateAvailableSlotDto dto)
        {
            try
            {
                var slot = await _availableSlotService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = slot.Id }, slot);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _availableSlotService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}