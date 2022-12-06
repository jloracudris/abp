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
    public interface ILotStatusService : IApplicationService
    {
        Task<PagedResultDto<LotStatusDto>> GetListAsync();
        Task<LotStatusDto> CreateAsync(string text);
        Task DeleteAsync(Guid id);
    }
}
