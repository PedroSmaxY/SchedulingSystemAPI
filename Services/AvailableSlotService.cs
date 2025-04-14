using SchedulingSystemAPI.DTOs.AvailableSlotDtos;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Repositories.Interfaces;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Services
{
    public class AvailableSlotService(IAvailableSlotRepository availableSlotRepository) : IAvailableSlotService
    {
        private readonly IAvailableSlotRepository _availableSlotRepository = availableSlotRepository;

        public async Task<IEnumerable<AvailableSlotDto>> GetAllAsync()
        {
            var slots = await _availableSlotRepository.GetAllAsync();
            return slots.Select(MapToDto);
        }

        public async Task<AvailableSlotDto?> GetByIdAsync(int id)
        {
            var slot = await _availableSlotRepository.GetByIdAsync(id);
            return slot != null ? MapToDto(slot) : null;
        }

        public async Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync()
        {
            var slots = await _availableSlotRepository.GetAvailableSlotsAsync();
            return slots.Select(MapToDto);
        }

        public async Task<IEnumerable<AvailableSlotDto>> GetByDateAsync(DateTime date)
        {
            var slots = await _availableSlotRepository.GetAvailableSlotsByDateAsync(date);
            return slots.Select(MapToDto);
        }

        public async Task<AvailableSlotDto> CreateAsync(CreateAvailableSlotDto dto)
        {
            // Validar que a data de início é antes da data de término
            if (dto.EndTime <= dto.StartTime)
                throw new ArgumentException("A data de término deve ser posterior à data de início");

            // Validar que o slot não está no passado
            if (dto.StartTime < DateTime.UtcNow)
                throw new ArgumentException("Não é possível criar slots no passado");

            // Verificar se há sobreposição com outros slots
            var existingSlots = await _availableSlotRepository.GetByDateRangeAsync(
                dto.StartTime.Date,
                dto.StartTime.Date.AddDays(1).AddTicks(-1));

            foreach (var slot in existingSlots)
            {
                // Verifica se há sobreposição
                if ((dto.StartTime >= slot.StartTime && dto.StartTime < slot.EndTime) ||
                    (dto.EndTime > slot.StartTime && dto.EndTime <= slot.EndTime) ||
                    (dto.StartTime <= slot.StartTime && dto.EndTime >= slot.EndTime))
                {
                    throw new InvalidOperationException("O horário solicitado se sobrepõe a um slot existente");
                }
            }

            // Criar o slot
            var newSlot = new AvailableSlot(dto.StartTime, dto.EndTime);

            await _availableSlotRepository.AddAsync(newSlot);

            return MapToDto(newSlot);
        }

        public async Task DeleteAsync(int id)
        {
            var slot = await _availableSlotRepository.GetByIdAsync(id);
            if (slot == null)
                throw new KeyNotFoundException("Horário não encontrado");

            // Verificar se o slot já está associado a um agendamento
            if (!slot.IsAvailable && slot.AppointmentId.HasValue)
                throw new InvalidOperationException("Não é possível excluir um horário que já está agendado");

            await _availableSlotRepository.DeleteAsync(id);
        }

        // Método auxiliar para mapear de AvailableSlot para AvailableSlotDto
        private static AvailableSlotDto MapToDto(AvailableSlot slot)
        {
            return new AvailableSlotDto
            {
                Id = slot.Id,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsAvailable = slot.IsAvailable,
                AppointmentId = slot.AppointmentId
            };
        }
    }
}