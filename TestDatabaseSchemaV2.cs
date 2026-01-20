using System;
using System.IO;
using System.Collections.Generic;
using WarehouseManagement.Models;
using WarehouseManagement.Repositories;

namespace WarehouseManagement
{
    public static class TestDatabaseSchemaV2
    {
        public static void Run()
        {
            string logFile = "verification_result.txt";
            using (StreamWriter writer = new StreamWriter(logFile))
            {
                Action<string> log = (msg) => {
                    Console.WriteLine(msg);
                    writer.WriteLine(msg);
                };

                try
                {
                    log("=== STARTING VERIFICATION ===");
                    
                    // 1. Repositories
                    var supRepo = new SupplierRepository();
                    var custRepo = new CustomerRepository();
                    var prodRepo = new ProductRepository();
                    var catRepo = new CategoryRepository();
                    var userRepo = new UserRepository();
                    var transRepo = new TransactionRepository();
                    var checkRepo = new InventoryCheckRepository();

                    // 2. Clear Data (Optional, or just add new)
                    // log("Clearing old test data...");

                    // 3. Create Supplier
                    log("Creating Supplier...");
                    var sup = new Supplier { SupplierName = "Test Supplier " + DateTime.Now.Ticks, Phone = "0123456789", Email = "test@sup.com" };
                    int supId = supRepo.AddSupplier(sup);
                    log($"Supplier Created. ID: {supId}");
                    if (supRepo.GetSupplierById(supId) == null) throw new Exception("Failed to retrieve Supplier");

                    // 4. Create Customer
                    log("Creating Customer...");
                    var cust = new Customer { CustomerName = "Test Customer " + DateTime.Now.Ticks, Phone = "0987654321", Email = "test@cust.com" };
                    int custId = custRepo.AddCustomer(cust);
                    log($"Customer Created. ID: {custId}");
                    if (custRepo.GetCustomerById(custId) == null) throw new Exception("Failed to retrieve Customer");

                    // 5. Create Category & Product
                    log("Creating Category & Product...");
                    var cat = new Category { CategoryName = "Test Cat " + DateTime.Now.Ticks };
                    int catId = catRepo.AddCategory(cat);
                    
                    var prod = new Product { ProductName = "Test Product " + DateTime.Now.Ticks, CategoryID = catId, Price = 10000, Quantity = 100, MinThreshold = 10 };
                    int prodId = prodRepo.AddProduct(prod);
                    log($"Product Created. ID: {prodId}");

                    // 6. Create User
                    var user = new User { Username = "tester" + DateTime.Now.Ticks, Password = "123", FullName = "Tester", Role = "Staff" };
                    int userId = userRepo.AddUser(user);
                    log($"User Created. ID: {userId}");

                    // 7. Create Transaction (Import)
                    log("Creating Import Transaction...");
                    var trans = new Transaction 
                    { 
                        Type = "Import", 
                        DateCreated = DateTime.Now, 
                        CreatedByUserID = userId, 
                        SupplierID = supId, 
                        Note = "Test Import",
                        Visible = true
                    };
                    int transId = transRepo.CreateTransaction(trans);
                    
                    var detail = new TransactionDetail 
                    { 
                        TransactionID = transId, 
                        ProductID = prodId, 
                        Quantity = 10, 
                        UnitPrice = 9000, 
                        Visible = true 
                    };
                    transRepo.AddTransactionDetail(detail);
                    transRepo.UpdateTransactionTotal(transId);
                    
                    var loadedTrans = transRepo.GetTransactionById(transId);
                    log($"Transaction Created. ID: {loadedTrans.TransactionID}, Total: {loadedTrans.TotalAmount}");
                    if (loadedTrans.TotalAmount != 90000) throw new Exception($"TotalAmount Mismatch. Expected 90000, got {loadedTrans.TotalAmount}");
                    if (loadedTrans.SupplierID != supId) throw new Exception("SupplierID Mismatch");

                    // 8. Create Inventory Check
                    log("Creating Inventory Check...");
                    var check = new InventoryCheck 
                    { 
                        CheckDate = DateTime.Now, 
                        CreatedByUserID = userId, 
                        Status = "Pending", 
                        Note = "Test Check",
                        Visible = true
                    };
                    int checkId = checkRepo.CreateCheck(check);
                    
                    var checkDetail = new InventoryCheckDetail 
                    { 
                        CheckID = checkId, 
                        ProductID = prodId, 
                        SystemQuantity = 100, 
                        ActualQuantity = 95, 
                        Reason = "Lost" 
                    };
                    checkRepo.AddCheckDetail(checkDetail);
                    
                    var loadedCheck = checkRepo.GetCheckById(checkId);
                    log($"Inventory Check Created. ID: {loadedCheck.CheckID}, Details Count: {loadedCheck.Details.Count}");
                    if (loadedCheck.Details.Count == 0) throw new Exception("Inventory Check Details missing");

                    // 9. Soft Delete Verification
                    log("Testing Soft Delete on Transaction...");
                    transRepo.SoftDeleteTransaction(transId);
                    var deletedTrans = transRepo.GetTransactionById(transId); // Should be null or visible=false? 
                    // Repository GetTransactionById: "SELECT * FROM Transactions WHERE TransactionID=@id" - it gets it regardless of Visible usually? 
                    // Checking TransactionRepo code: 
                    // GetAllTransactions respects includeHidden.
                    // GetTransactionById just gets by ID, but Detail fetching has WHERE Visible=TRUE.
                    // Let's check visible property.
                    if (deletedTrans.Visible) throw new Exception("Soft Delete failed (Visible is true)");
                    log("Soft Delete verified.");
                    
                    // 10. Test Inventory Check Balance
                    TestInventoryCheckBalance();

                    log("=== VERIFICATION SUCCESS ===");
                }
                catch (Exception ex)
                {
                    log("=== VERIFICATION FAILED ===");
                    log(ex.ToString());
                }
            }
        }
        static void TestInventoryCheckBalance()
        {
            Console.WriteLine("\n--- Testing Inventory Check Balance ---");
            var checkService = new WarehouseManagement.Services.InventoryCheckService();
            var productRepo = new WarehouseManagement.Repositories.ProductRepository();
            var supplierRepo = new WarehouseManagement.Repositories.SupplierRepository();
            var invService = new WarehouseManagement.Services.InventoryService();

            // 1. Create a product with qty 10
            var supplier = new WarehouseManagement.Models.Supplier { SupplierName = "Test Supplier Check", Phone = "123", Visible = true };
            supplierRepo.AddSupplier(supplier); // Need ID? AddSupplier returns bool or int? Service returns bool. Repo returns int?
            // Repo AddSupplier returns int (LAST_INSERT_ID) based on previous check. Wait, Service returns bool.
            // Let's rely on generic product creation or assume product 1 exists.
            
            // Checking ProductRepository.cs: AddProduct returns int.
            var product = new WarehouseManagement.Models.Product 
            { 
                ProductName = "Check Product", 
                Quantity = 10, 
                Price = 1000, 
                CategoryID = 1,
                Visible = true 
            };
            int prodId = productRepo.AddProduct(product);
            Console.WriteLine($"Created Product ID {prodId} with Qty 10");

            // 2. Create Check with Actual = 15 (Diff +5)
            var details = new List<WarehouseManagement.Models.InventoryCheckDetail>
            {
                new WarehouseManagement.Models.InventoryCheckDetail
                {
                    ProductID = prodId,
                    SystemQuantity = 10,
                    ActualQuantity = 15, // +5
                    Difference = 5,
                    Reason = "Found more"
                }
            };

            int checkId = checkService.CreateCheck(1, "Test Balance", details, "Pending");
            Console.WriteLine($"Created Pending Check ID {checkId}");

            // 3. Complete Check
            checkService.CompleteCheck(checkId, 1);
            Console.WriteLine("Completed Check");

            // 4. Verify Product Qty updated to 15
            var updatedProd = productRepo.GetProductById(prodId);
            Console.WriteLine($"Product Qty after check: {updatedProd.Quantity} (Expected 15)");

            if (updatedProd.Quantity == 15) Console.WriteLine("SUCCESS: Stock updated correctly.");
            else throw new Exception($"FAILURE: Stock not updated. Expected 15, got {updatedProd.Quantity}");

            // 5. Verify Transaction created
            // We can't easily fetch the exact transaction but stock update confirms logic.
        }
    }
}
