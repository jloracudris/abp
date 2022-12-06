using Americasa.Demo.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Americasa.Demo.Interfaces
{
    public interface IDealerShipService : IApplicationService
    {
        Task<PagedResultDto<DealerShipDto>> GetListAsync();
        Task<DealerShipDto> CreateAsync(string text);
        Task DeleteAsync(Guid id);
    }
}
