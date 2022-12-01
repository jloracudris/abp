using Americasa.Demo.Dto;
using Americasa.Demo.Entities.Entities;
using Americasa.Demo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Americasa.Demo
{
    public class HouseStatusService : ApplicationService, IHouseStatusService
    {
        private readonly IRepository<HouseStatus, Guid> _houseStatusRepository;

        public HouseStatusService(IRepository<HouseStatus, Guid> houseStatusRepository)
        {
            _houseStatusRepository = houseStatusRepository;
        }

        public async Task<HouseStatusDto> CreateAsync(string text)
        {
            var houseStatus = await _houseStatusRepository.InsertAsync(
                new HouseStatus { Name = text }
            );

            return new HouseStatusDto
            {
                Name = houseStatus.Name,
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _houseStatusRepository.DeleteAsync(id);
        }

        public async Task<List<HouseStatusDto>> GetListAsync()
        {
            var items = await _houseStatusRepository.GetListAsync();
            return items
                .Select(item => new HouseStatusDto
                {
                    Name = item.Name,
                }).ToList();
        }
    }
}
