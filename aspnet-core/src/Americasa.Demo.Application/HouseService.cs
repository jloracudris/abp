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
    public class HouseService : ApplicationService, IHouseService
    {
        private readonly IRepository<House, Guid> _houseRepository;

        public HouseService(IRepository<House, Guid> houseRepository)
        {
            _houseRepository = houseRepository;
        }

        public async Task<HouseDto> CreateAsync(string text)
        {
            var house = await _houseRepository.InsertAsync(
                new House { Name = text, HomeStatusId = 1 }
            );

            return new HouseDto
            {
                Name = house.Name,
                HomeStatusId = house.HomeStatusId,
                HomeId = house.HomeId,
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _houseRepository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<HouseDto>> GetListAsync()
        {
            var items = await _houseRepository.GetListAsync();
            var totalCount = await _houseRepository.GetCountAsync();

            var rs = items
                .Select(item => new HouseDto
                {
                    Name = item.Name,
                    HomeStatusId = item.HomeStatusId,
                    HomeId = item.HomeId,
                }).ToList();

            return new PagedResultDto<HouseDto>(
                totalCount,
                rs
            );
        }
    }
}
