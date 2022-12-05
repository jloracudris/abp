using Americasa.Demo.Dto;
using Americasa.Demo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Americasa.Demo.Interfaces
{
    public interface IHouseDealService : IApplicationService
    {
        Task<PagedResultDto<HouseDealDto>> GetListAsync();
        Task<HouseDealDto> CreateAsync(string text);
        Task DeleteAsync(Guid id);
    }
}
