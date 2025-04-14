using SchedulingSystemAPI.DTOs.AppointmentDtos;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Models.Enums;
using SchedulingSystemAPI.Repositories.Interfaces;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Services
{
    public class AppointmentService(
        IAppointmentRepository appointmentRepository,
        IUserRepository userRepository,
        IServiceRepository serviceRepository,
        IAvailableSlotRepository availableSlotRepository) : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IServiceRepository _serviceRepository = serviceRepository;
        private readonly IAvailableSlotRepository _availableSlotRepository = availableSlotRepository;

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            return appointments.Select(MapToDto);
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            return appointment != null ? MapToDto(appointment) : null;
        }

        public async Task<IEnumerable<AppointmentDto>> GetByUserIdAsync(int userId)
        {
            var appointments = await _appointmentRepository.GetByUserIdAsync(userId);
            return appointments.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            var appointments = await _appointmentRepository.GetByDateRangeAsync(start, end);
            return appointments.Select(MapToDto);
        }

        public async Task<IEnumerable<AppointmentDto>> GetByStatusAsync(Status status)
        {
            var appointments = await _appointmentRepository.GetByStatusAsync(status);
            return appointments.Select(MapToDto);
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            // Verificar se o usuário existe
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado");

            // Verificar se o serviço existe
            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId);
            if (service == null)
                throw new KeyNotFoundException("Serviço não encontrado");

            // Verificar se o slot está disponível
            var slot = await _availableSlotRepository.GetByIdAsync(dto.AvailableSlotId);
            if (slot == null)
                throw new KeyNotFoundException("Horário não encontrado");

            if (!slot.IsAvailable)
                throw new InvalidOperationException("O horário selecionado não está mais disponível");

            // Criar o agendamento
            var appointment = new Appointment
            {
                UserId = dto.UserId,
                ServiceId = dto.ServiceId,
                AvailableSlotId = dto.AvailableSlotId,
                AppointmentDateTime = slot.StartTime,
                Notes = dto.Notes,
                Status = Status.Pending
            };

            // Reservar o slot
            slot.Reserve(appointment.Id);

            await _appointmentRepository.AddAsync(appointment);

            // Buscar o agendamento completo para retornar
            var createdAppointment = await _appointmentRepository.GetByIdAsync(appointment.Id);

            // Verificar se o agendamento existe após a criação
            if (createdAppointment == null)
                throw new InvalidOperationException("Falha ao recuperar o agendamento após criação");

            return MapToDto(createdAppointment);
        }

        public async Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException("Agendamento não encontrado");

            // Atualizar apenas os campos fornecidos
            if (dto.Status.HasValue)
                appointment.Status = dto.Status.Value;

            if (dto.Notes != null)
                appointment.Notes = dto.Notes;

            await _appointmentRepository.UpdateAsync(appointment);

            // Buscar o agendamento atualizado
            var updatedAppointment = await _appointmentRepository.GetByIdAsync(id) ?? throw new InvalidOperationException("Falha ao recuperar o agendamento após atualização");
            return MapToDto(updatedAppointment);
        }

        public async Task CancelAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException("Agendamento não encontrado");

            // Verificar se o agendamento já está cancelado
            if (appointment.Status == Status.Canceled)
                return;

            // Cancelar o agendamento
            appointment.Status = Status.Canceled;

            // Liberar o slot se estiver associado
            if (appointment.AvailableSlot != null)
            {
                appointment.AvailableSlot.Release();
            }

            await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task ConfirmAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException("Agendamento não encontrado");

            // Verificar se o agendamento é confirmável
            if (appointment.Status != Status.Pending)
                throw new InvalidOperationException("Apenas agendamentos pendentes podem ser confirmados");

            // Confirmar o agendamento
            appointment.Status = Status.Confirmed;

            await _appointmentRepository.UpdateAsync(appointment);
        }

        // Helper método para mapear de Appointment para AppointmentDto
        private static AppointmentDto MapToDto(Appointment appointment)
        {
            return new AppointmentDto
            {
                Id = appointment.Id,
                UserId = appointment.UserId,
                User = appointment.User != null ? new DTOs.UserDtos.UserDto
                {
                    Id = appointment.User.Id,
                    Name = appointment.User.Name,
                    Email = appointment.User.Email,
                    Role = appointment.User.Role,
                    IsActive = appointment.User.IsActive
                } : null,
                ServiceId = appointment.ServiceId,
                Service = appointment.Service != null ? new DTOs.ServiceDtos.ServiceDto
                {
                    Id = appointment.Service.Id,
                    Name = appointment.Service.Name,
                    Duration = appointment.Service.Duration,
                    Description = appointment.Service.Description,
                    IsActive = appointment.Service.IsActive,
                    CreatedAt = appointment.Service.CreatedAt,
                    UpdatedAt = appointment.Service.UpdatedAt
                } : null,
                AvailableSlotId = appointment.AvailableSlotId,
                AppointmentDateTime = appointment.AppointmentDateTime,
                Status = appointment.Status,
                Notes = appointment.Notes,
                CreatedAt = appointment.CreatedAt,
                UpdatedAt = appointment.UpdatedAt
            };
        }
    }
}