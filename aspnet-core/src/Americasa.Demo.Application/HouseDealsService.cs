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
    public class HouseDealsService : ApplicationService, IHouseDealService
    {
        private readonly IRepository<HouseDeal, Guid> _houseDealsRepository;

        public HouseDealsService(IRepository<HouseDeal, Guid> houseDealsRepository)
        {
            _houseDealsRepository = houseDealsRepository;
        }

        public async Task<HouseDealDto> CreateAsync(string text)
        {
            var deal = await _houseDealsRepository.InsertAsync(
                new HouseDeal { Name = text }
            );

            return new HouseDealDto
            {   
                Name = deal.Name,
                Attachment = deal.Attachment,
                BoxSize = deal.BoxSize,
                CreationTime = DateTime.Now,
                DealId = deal.DealId,
                Email = deal.Email,
                HomeStatusId = deal.HomeStatusId,                
                HouseName = deal.HouseName,                
                LotNumber = deal.LotNumber,
                LotStatusId = deal.LotStatusId,
                PhoneNumber = deal.PhoneNumber,
                UpdateTime = DateTime.Now,
                WindZone = deal.WindZone,
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _houseDealsRepository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<HouseDealDto>> GetListAsync()
        {
            var deals = await _houseDealsRepository.GetListAsync();
            //Get the total count with another query
            var totalCount = await _houseDealsRepository.GetCountAsync();


            var rs = deals
                .Select(deal => new HouseDealDto
                {
                    Name = deal.Name,
                    Attachment = deal.Attachment,
                    BoxSize = deal.BoxSize,
                    CreationTime = DateTime.Now,
                    DealId = deal.DealId,
                    Email = deal.Email,
                    HomeStatusId = deal.HomeStatusId,
                    HouseName = deal.HouseName,
                    LotNumber = deal.LotNumber,
                    LotStatusId = deal.LotStatusId,
                    PhoneNumber = deal.PhoneNumber,
                    UpdateTime = DateTime.Now,
                    WindZone = deal.WindZone,
                }).ToList();

            return new PagedResultDto<HouseDealDto>(
                totalCount,
                rs
            );
        }
    }
}
