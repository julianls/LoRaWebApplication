using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoraWebUI.Models;
using LoraWebUI.Models.DeviceViewModels;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;

namespace LoraWebUI.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration config;

        public HomeController(IConfiguration config)
        {
            this.config = config;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Devices = GetDeviecs();
            homeViewModel.DataLines = new List<DataLine>();
            return View(homeViewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Index(HomeViewModel model)
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Devices = GetDeviecs();
            homeViewModel.DataLines = GetDataLines(model.DeviceId);
            homeViewModel.DeviceId = model.DeviceId;

            return View("Index", homeViewModel);
        }

        private List<DeviceViewModel> GetDeviecs()
        {
            string registryConnectionString = config["AzureConnection:RegistryConnectionString"];
            RegistryManager registryManager = RegistryManager.CreateFromConnectionString(registryConnectionString);
            List<DeviceViewModel> devices = new List<DeviceViewModel>();
            foreach (var item in registryManager.GetDevicesAsync(100).Result)
            {
                devices.Add(new DeviceViewModel() { Id = item.Id, Name = item.Id });
            }
            return devices;
        }

        public List<DataLine> GetDataLines(string deviceId)
        {
            // ADD THIS PART TO YOUR CODE
            string EndpointUri = config["AzureConnection:DBEndpointUri"];
            string PrimaryKey = config["AzureConnection:DBPrimaryKey"];
            DocumentClient client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);

            // Set some common query options
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            string databaseName = config["AzureConnection:DBName"];
            string collectionName = config["AzureConnection:DBCollection"];

            // Now execute the same query via direct SQL
            IQueryable<DataLine> dataLines = client.CreateDocumentQuery<DataLine>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                    $"SELECT * FROM c WHERE c.ConnectionDeviceId = \"{deviceId}\"", queryOptions);

            return dataLines.ToList();
        }
    }
}
