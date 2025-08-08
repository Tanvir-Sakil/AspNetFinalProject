using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ISqsRepository
    {
        Task SendResizeRequestAsync(string fileName,string uploader,string imageType,string processorName);
    }
}
