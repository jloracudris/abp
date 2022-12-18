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

        public async Task<HouseDealDto> CreateAsync(string name, string attachment, string boxsize, string email, string houseName, string lotNumber, string phone, string windZone)
        {
            var deal = await _houseDealsRepository.InsertAsync(
                new HouseDeal
                {
                    Name = name,
                    Attachment = attachment,
                    BoxSize = boxsize,
                    CreationTime = DateTime.Now,
                    DealId = Guid.NewGuid(),
                    Email = email,
                    HomeStatusId = Guid.NewGuid(),
                    HouseName = houseName,
                    LotNumber = lotNumber,
                    LotStatusId = Guid.NewGuid(),
                    PhoneNumber = phone,
                    WindZone = windZone,
                    UpdateTime = DateTime.Now,
                }
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
            try
            {
                var deals2 = await _houseDealsRepository.GetListAsync();
            }
            catch (Exception e)
            {

                throw;
            }
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
                    InstanceId = deal.InstanceId
                }).ToList();

            return new PagedResultDto<HouseDealDto>(
                totalCount,
                rs
            );
        }
    }
}
