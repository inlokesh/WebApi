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

    public class Device
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
    }

    public static class Extensions
    {
        public static Device ToDevice(this DeviceEntity deviceEntity)
        {
            var result = new Device();
            result.DeviceID = deviceEntity.DeviceID;
            result.DeviceName = deviceEntity.DeviceName;
            result.DeviceType = deviceEntity.Type.ToString();
            return result;
        }
    }

    public enum DeviceType { Camera, Thermostat, SecurityPanel }
}