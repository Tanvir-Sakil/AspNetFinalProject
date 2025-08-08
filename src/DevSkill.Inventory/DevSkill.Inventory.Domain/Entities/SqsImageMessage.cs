using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class SqsImageMessage
    {
        public string FileName { get; set; }
        public string Uploader { get; set; }
        public string ImageType { get; set; }
        public string ProcessorName { get; set; }
    }

}
