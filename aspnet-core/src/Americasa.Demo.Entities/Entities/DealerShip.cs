using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace Americasa.Demo.Entities.Entities
{
    public class DealerShip : Entity<Guid>
    {
        public string Name { get; set; }
        public Guid DealerShipId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public DealerShip() { }
    }
}
