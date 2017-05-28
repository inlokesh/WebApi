using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebApi.Models;
namespace WebApi.Controllers
{
    public class DeviceController : ApiController
    {
        // GET: api/Device
        [Route("GetAllDevices")]
        public List<Device> Get()
        {
            var tableController = new TableStorageController();
            tableController.Init();
            tableController.RemoveDevices();
            tableController.InsertDeviceAsync();
            var result = tableController.GetAllDevices();
            return result;
        }

        // GET: api/Device/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Device
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Device/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Device/5
        public void Delete(int id)
        {
        }
    }
}
