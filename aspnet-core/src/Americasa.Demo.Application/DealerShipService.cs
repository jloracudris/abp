using Americasa.Demo.Dto;
using Americasa.Demo.Entities.Entities;
using Americasa.Demo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Americasa.Demo
{
    public class DealerShipService : ApplicationService, IDealerShipService
    {
        private readonly IRepository<DealerShip, Guid> _dealerShipRepository;

        public DealerShipService(IRepository<DealerShip, Guid> dealerShipRepository)
        {
            _dealerShipRepository = dealerShipRepository;
        }

        public async Task<DealerShipDto> CreateAsync(string text)
        {
            var dealerShip = await _dealerShipRepository.InsertAsync(
                new DealerShip { Name = text }
            );

            return new DealerShipDto
            {
                DealerShipId = dealerShip.DealerShipId,
                Name = dealerShip.Name
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _dealerShipRepository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<DealerShipDto>> GetListAsync()
        {
            var items = await _dealerShipRepository.GetListAsync();
            var totalCount = await _dealerShipRepository.GetCountAsync();

            var rs = items
                .Select(item => new DealerShipDto
                {
                    DealerShipId = item.DealerShipId,
                    Name = item.Name
                }).ToList();

            return new PagedResultDto<DealerShipDto>(
                totalCount,
                rs
            );
        }
    }
}
