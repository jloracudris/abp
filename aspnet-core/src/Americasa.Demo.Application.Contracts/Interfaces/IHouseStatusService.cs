using Americasa.Demo.Dto;
using Americasa.Demo.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Americasa.Demo.Interfaces
{
    public interface IHouseStatusService : IApplicationService
    {
        Task<List<HouseStatusDto>> GetListAsync();
        Task<HouseStatusDto> CreateAsync(string text);
        Task DeleteAsync(Guid id);
    }
}
