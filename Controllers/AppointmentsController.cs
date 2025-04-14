using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchedulingSystemAPI.DTOs.AppointmentDtos;
using SchedulingSystemAPI.Models.Enums;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAll()
        {
            var appointments = await _appointmentService.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentDto>> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();

            // Verificar se o usuário pode ver este agendamento
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            if (!User.IsInRole("Admin") && appointment.UserId != userId)
                return Forbid();

            return Ok(appointment);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByUserId(int userId)
        {
            // Verificar se o usuário pode ver estes agendamentos
            var currentUserId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            if (!User.IsInRole("Admin") && userId != currentUserId)
                return Forbid();

            var appointments = await _appointmentService.GetByUserIdAsync(userId);
            return Ok(appointments);
        }

        [HttpGet("date-range")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByDateRange(
            [FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var appointments = await _appointmentService.GetByDateRangeAsync(start, end);
            return Ok(appointments);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByStatus(Status status)
        {
            var appointments = await _appointmentService.GetByStatusAsync(status);
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto dto)
        {
            try
            {
                if (!User.IsInRole("Admin"))
                {
                    var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                    dto.UserId = userId;
                }

                var appointment = await _appointmentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AppointmentDto>> Update(int id, UpdateAppointmentDto dto)
        {
            try
            {
                // Verificar se o usuário pode atualizar este agendamento
                var appointment = await _appointmentService.GetByIdAsync(id);
                if (appointment == null)
                    return NotFound();

                var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                if (!User.IsInRole("Admin") && appointment.UserId != userId)
                    return Forbid();

                var updatedAppointment = await _appointmentService.UpdateAsync(id, dto);
                return Ok(updatedAppointment);
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

        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> Cancel(int id)
        {
            try
            {
                // Verificar se o usuário pode cancelar este agendamento
                var appointment = await _appointmentService.GetByIdAsync(id);
                if (appointment == null)
                    return NotFound();

                var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
                if (!User.IsInRole("Admin") && appointment.UserId != userId)
                    return Forbid();

                await _appointmentService.CancelAsync(id);
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

        [HttpPut("{id}/confirm")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Confirm(int id)
        {
            try
            {
                await _appointmentService.ConfirmAsync(id);
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