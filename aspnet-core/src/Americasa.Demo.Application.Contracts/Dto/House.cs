using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Americasa.Demo.Entities.Entities
{
    public class HouseDto
    {
        public Guid HomeId { get; set; }
        public string Name { get; set; }
        public int HouseStatusId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }

    }
}
