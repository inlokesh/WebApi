using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

using WebApi.Models;

namespace WebApi.Controllers
{
    public class TableStorageController
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings.Get("StorageConnectionString"));
        public TableStorageController()
        {
        }
        CloudTable devicesTable;
        public void Init()
        {
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Get a reference to a table named "DevicesTable"
            devicesTable = tableClient.GetTableReference("DevicesTable");
            // Create the CloudTable if it does not exist
            devicesTable.CreateIfNotExists();
        }

        public void InsertDeviceAsync()
        {
            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            // Create a customer entity and add it to the table.
            DeviceEntity device1 = new DeviceEntity();
            device1.DeviceID = 1;
            device1.DeviceName = "Vacation Home Camera 1";
            device1.Type = DeviceType.Camera;
            device1.AssignPartitionKey();
            device1.AssignRowKey();

            // Create another customer entity and add it to the table.
            DeviceEntity device2 = new DeviceEntity();
            device2.DeviceID = 2;
            device2.DeviceName = "City Home Thermostat 1";
            device2.Type = DeviceType.Thermostat;
            device2.AssignPartitionKey();
            device2.AssignRowKey();

            if (RetrieveRecord(devicesTable, device1.Type.ToString(), device1.DeviceID.ToString()) == null)
            // Add both customer entities to the batch insert operation.
            {
                TableOperation tableOperation = TableOperation.Insert(device1);
                devicesTable.Execute(tableOperation);
            }

            if (RetrieveRecord(devicesTable, device2.Type.ToString(), device2.DeviceID.ToString()) == null)
            {
                TableOperation tableOperation = TableOperation.Insert(device2);
                devicesTable.Execute(tableOperation);
            }
        }

        public void RemoveDevices()
        {
            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();

            // Create a device entity and add it to the table.
            DeviceEntity device1 = new DeviceEntity();
            device1 = RetrieveRecord(devicesTable, DeviceType.Camera.ToString(), "1");

            // Create another device entity and add it to the table.
            DeviceEntity device2 = new DeviceEntity();
            device2 = RetrieveRecord(devicesTable, DeviceType.Camera.ToString(), "2");

            // Create another customer entity and add it to the table.
            DeviceEntity device3 = new DeviceEntity();
            device3 = RetrieveRecord(devicesTable, DeviceType.Camera.ToString(), "3");

            if (device1 != null)
            {
                TableOperation tableOperation = TableOperation.Delete(device1);
                devicesTable.Execute(tableOperation);
            }

            if (device2 != null)
            {
                TableOperation tableOperation = TableOperation.Delete(device2);
                devicesTable.Execute(tableOperation);
            }

            if (device3 != null)
            {
                TableOperation tableOperation = TableOperation.Insert(device3);
                devicesTable.Execute(tableOperation);
            }
        }

        public DeviceEntity RetrieveRecord(CloudTable table, string partitionKey, string rowKey)
        {
            TableOperation tableOperation = TableOperation.Retrieve<DeviceEntity>(partitionKey, rowKey);
            TableResult tableResult = table.Execute(tableOperation);
            return tableResult.Result as DeviceEntity;
        }
        public List<Device> GetAllDevices()
        {
            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<DeviceEntity> query = new TableQuery<DeviceEntity>();
            //Declare Result object
            List<Device> result = new List<Device>();

            // Print the fields for each customer.
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<DeviceEntity> resultSegment = devicesTable.ExecuteQuerySegmented(query, token);
                token = resultSegment.ContinuationToken;

                foreach (DeviceEntity entity in resultSegment.Results)
                {
                    result.Add(entity.ToDevice());
                }
            } while (token != null);
            return result;
        }
    }
}