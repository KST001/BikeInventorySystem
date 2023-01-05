using System.Text.Json;
using BikeInventorySystem.Data;

namespace BikeInventorySystem.Data
{
    /* InventoryService Method*/
    public static class InventoryService
    {

        /* SaveAll Method*/
        private static void SaveAll(Guid userId,List<InventoryItems> inventoryItems)
        {
            string appDataDirectoryPath = Utils.GetAppDirectoryPath();
            string appInventoryFilePath = Utils.GetInventoryFilePath();

            if (!Directory.Exists(appDataDirectoryPath))
            {
                Directory.CreateDirectory(appDataDirectoryPath);
            }

            var json = JsonSerializer.Serialize(inventoryItems);
            File.WriteAllText(appInventoryFilePath, json);
        }

        /* GetAll Method*/
        public static List<InventoryItems> GetAll()
        {
            string appInventoryFilePath = Utils.GetInventoryFilePath();
            if (!File.Exists(appInventoryFilePath))
            {
                return new List<InventoryItems>();
            }

            var json = File.ReadAllText(appInventoryFilePath);

            return JsonSerializer.Deserialize<List<InventoryItems>>(json);
        }



        /* Create Method*/
        public static List<InventoryItems> Create(Guid userId,string  addedBy, string itemName, int quantity)
        {
            if (quantity <=0)
            {
                throw new Exception("Cannot Add Item with 0 quantity");

            }
            else if (itemName == null)
            {
                throw new Exception("Item Name cannot be empty");

            }
            else if (itemName.GetType()!=typeof(string))
            {
                throw new Exception("Item Name must be string");
            }
            else if (quantity.GetType()!=typeof(int))
            {
                throw new Exception("Quantity must be integer");
            }
            else
            {
                List<InventoryItems> inventoryItems = GetAll();
                inventoryItems.Add(new InventoryItems
                {
                    AddedBy = addedBy,
                    ItemName = itemName,
                    Quantity = quantity,
                });
                SaveAll(userId, inventoryItems);
                return inventoryItems;

            }
            
        }


        /* Update Method*/

        public static List<InventoryItems> Update(Guid userId,Guid id, string itemName, int quantity)
        {
            
            List<InventoryItems> items = GetAll();
            InventoryItems itemUpdate = items.FirstOrDefault(x => x.Id == id);

            if (itemUpdate == null)
            {
                throw new Exception("Item not found.");
            }
            else if (quantity <= 0)
            {
                throw new Exception("Cannot Add Item with 0 quantity");

            }
            else if (itemName == null)
            {
                throw new Exception("Item Name cannot be empty");

            }
            else if (itemName.GetType() != typeof(string))
            {
                throw new Exception("Item Name must be string");
            }
            else if (quantity.GetType() != typeof(int))
            {
                throw new Exception("Quantity must be integer");
            }
            else
            {
                itemUpdate.ItemName = itemName;
                itemUpdate.Quantity = quantity;
                SaveAll(userId, items);
                return items;

            }

            
        }


        /* WithdrawItem Method*/
        public static List<InventoryItems> WithdrawItem(Guid userId,Guid id, string itemName, int quantity)
        {
            DateTime currentTime = DateTime.Now;


            if (currentTime.Hour >= 9 && currentTime.Hour < 16 && currentTime.DayOfWeek >= DayOfWeek.Monday && currentTime.DayOfWeek <= DayOfWeek.Friday)
            {


                List<InventoryItems> items = GetAll();
                InventoryItems itemUpdate = items.FirstOrDefault(x => x.Id == id);

                if (itemUpdate == null)
                {
                    throw new Exception("Item not found.");
                }
                if (quantity <= 0)
                {
                    throw new Exception("Quantity cannot be 0 or less");
                }
                else
                {
                    itemUpdate.ItemName = itemName;
                    itemUpdate.Quantity = itemUpdate.Quantity - quantity;
                    SaveAll(userId, items);
                    return items;
                }


                
            }
            else 
            { 
                throw new Exception("The user cannot withdraw during this time.");
            }
        }


        /* RejectWithdrawItem Method*/
        public static List<InventoryItems> RejectWithdrawItem(Guid userId,Guid id, string itemName, int quantity)
        {
            List<InventoryItems> items = GetAll();
            InventoryItems itemUpdate = items.FirstOrDefault(x => x.Id == id);

            if (itemUpdate == null)
            {
                throw new Exception("Item not found.");
            }
            else
            {
                itemUpdate.ItemName = itemName;
                itemUpdate.Quantity += quantity;
                SaveAll(userId,items);
                return items;
            }
        }


        /* CancelWithdrawItem Method*/
        public static List<InventoryItems> CancelWithdrawItem(Guid userId,Guid id, string itemName, int quantity)
        {
            List<InventoryItems> items = GetAll();
            InventoryItems itemUpdate = items.FirstOrDefault(x => x.Id == id);

            if (itemUpdate == null)
            {
                throw new Exception("Item not found.");
            }

            itemUpdate.ItemName = itemName;
            itemUpdate.Quantity = itemUpdate.Quantity + quantity;
            SaveAll(userId, items);
            return items;
        }


        /* Delete Method*/
        public static List<InventoryItems> Delete(Guid userId,Guid id)
        {
            List<InventoryItems> items = GetAll();
            InventoryItems itemUpdate = items.FirstOrDefault(x => x.Id == id);

            if (itemUpdate == null)
            {
                throw new Exception("Item not found.");
            }

            items.Remove(itemUpdate);
            SaveAll(userId, items);
            return items;
        }
    }
}
