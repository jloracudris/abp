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
    public class LotStatusService : ApplicationService, ILotStatusService
    {
        private readonly IRepository<LotStatus, Guid> _lotStatusRepository;

        public LotStatusService(IRepository<LotStatus, Guid> lotStatusRepository)
        {
            _lotStatusRepository = lotStatusRepository;
        }

        public async Task<LotStatusDto> CreateAsync(string text)
        {
            var lotStatus = await _lotStatusRepository.InsertAsync(
                new LotStatus { Name = text }
            );

            return new LotStatusDto
            {
                Name = lotStatus.Name,
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _lotStatusRepository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<LotStatusDto>> GetListAsync()
        {
            var items = await _lotStatusRepository.GetListAsync();
            var totalCount = await _lotStatusRepository.GetCountAsync();

            var rs = items
                .Select(item => new LotStatusDto
                {
                    Name = item.Name,
                    LotStatusId = item.LotStatusId,
                }).ToList();

            return new PagedResultDto<LotStatusDto>(
                totalCount,
                rs
            );
        }
    }
}
