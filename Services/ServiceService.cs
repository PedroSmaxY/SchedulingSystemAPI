using SchedulingSystemAPI.DTOs.ServiceDtos;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Repositories.Interfaces;
using SchedulingSystemAPI.Services.Interfaces;

namespace SchedulingSystemAPI.Services
{
    public class ServiceService(IServiceRepository serviceRepository) : IServiceService
    {
        private readonly IServiceRepository _serviceRepository = serviceRepository;

        public async Task<IEnumerable<ServiceDto>> GetAllAsync(bool includeInactive = false)
        {
            var services = await _serviceRepository.GetAllAsync();

            if (!includeInactive)
                services = services.Where(s => s.IsActive);

            return services.Select(MapToDto);
        }

        public async Task<ServiceDto?> GetByIdAsync(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            return service != null ? MapToDto(service) : null;
        }

        public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
        {
            var existingService = await _serviceRepository.GetByNameAsync(dto.Name);
            if (existingService != null)
                throw new InvalidOperationException("Já existe um serviço com este nome");

            var service = new Service(dto.Name, dto.Duration, dto.Description);

            await _serviceRepository.AddAsync(service);
            return MapToDto(service);
        }

        public async Task<ServiceDto> UpdateAsync(int id, UpdateServiceDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(id);
            if (service == null)
                throw new KeyNotFoundException("Serviço não encontrado");

            if (!string.IsNullOrEmpty(dto.Name) && dto.Name != service.Name)
            {
                var existingService = await _serviceRepository.GetByNameAsync(dto.Name);
                if (existingService != null)
                    throw new InvalidOperationException("Já existe um serviço com este nome");
            }

            if (!string.IsNullOrEmpty(dto.Name))
                service.Name = dto.Name;

            if (dto.Duration.HasValue)
                service.Duration = dto.Duration.Value;

            if (dto.Description != null)
                service.Description = dto.Description;

            if (dto.IsActive.HasValue)
                service.IsActive = dto.IsActive.Value;

            await _serviceRepository.UpdateAsync(service);
            return MapToDto(service);
        }

        public async Task DeleteAsync(int id)
        {
            await _serviceRepository.DeleteAsync(id);
        }

        private static ServiceDto MapToDto(Service service)
        {
            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Duration = service.Duration,
                Description = service.Description,
                IsActive = service.IsActive,
                CreatedAt = service.CreatedAt,
                UpdatedAt = service.UpdatedAt
            };
        }
    }
}