using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure.Repositories;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ImageResizeQueueRepository : Repository<ImageResizeQueue, Guid>, IImageResizeQueueRepository
    {
        public ImageResizeQueueRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
