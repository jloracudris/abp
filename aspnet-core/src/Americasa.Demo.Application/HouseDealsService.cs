using Americasa.Demo.Dto;
using Americasa.Demo.Entities.Entities;
using Americasa.Demo.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class HouseDealsService : ApplicationService, IHouseDealService
    {
        private readonly IRepository<HouseDeal, Guid> _houseDealsRepository;

        public HouseDealsService(IRepository<HouseDeal, Guid> houseDealsRepository)
        {
            _houseDealsRepository = houseDealsRepository;
        }

        [Authorize("Americasa_Deals_Create")]
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

        [Authorize("Americasa_Deals_View")]
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
                    InstanceId = deal.InstanceId,
                    IsPublished = deal.IsPublished
                }).ToList();

            return new PagedResultDto<HouseDealDto>(
                totalCount,
                rs
            );
        }
    }
}
