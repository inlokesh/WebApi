using Microsoft.WindowsAzure.Storage.Table;

namespace WebApi.Models
{
    public class DeviceEntity : TableEntity
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public DeviceType Type { get; set; }
        public void AssignRowKey()
        {
            this.RowKey = this.DeviceID.ToString();
        }
        public void AssignPartitionKey()
        {
            this.PartitionKey = this.Type.ToString();
        }

    }

    public enum DeviceType { Camera, Thermostat, SecurityPanel }
}