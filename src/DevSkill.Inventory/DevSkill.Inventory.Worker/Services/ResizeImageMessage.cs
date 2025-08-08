using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Worker.Services
{
    public class ResizeImageMessage
    {
        public string FileName { get; set; }
        public string ProcessorName { get; set; }
    }
}
