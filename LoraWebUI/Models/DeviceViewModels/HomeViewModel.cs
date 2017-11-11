using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoraWebUI.Models.DeviceViewModels
{
    public class HomeViewModel
    {
        public List<DeviceViewModel> Devices { get; set; }

        public string DeviceId { get; set; }

        public List<DataLine> DataLines { get; set; }
    }
}
