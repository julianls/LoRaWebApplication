using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoraWebUI.Models
{
    public class DataLine
    {
        public string ConnectionDeviceId { get; set; }

        public string AppId { get; set; }

        public string rawData { get; set; }

        public string EnqueuedTime { get; set; }
    }
}
