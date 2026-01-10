using System;
using System.Collections.Generic;
using WarehouseManagement.Controllers;
using WarehouseManagement.Models;
using WarehouseManagement.Helpers;

// Quick test to verify transaction detail loading works
try
{
    Console.WriteLine("[Test] Initializing test...");
    
    // Test database connection
    if (!DatabaseHelper.TestDatabaseConnection())
    {
        Console.WriteLine("[Test] Database connection failed!");
        return;
    }
    
    Console.WriteLine("[Test] Database connection successful!");
    
    // Create controller
    var controller = new InventoryController();
    Console.WriteLine("[Test] InventoryController created");
    
    // Get all transactions
    var transactions = controller.GetAllTransactions();
    Console.WriteLine($"[Test] Found {transactions.Count} transactions");
    
    if (transactions.Count > 0)
    {
        // Try to get first transaction details
        var firstTrans = transactions[0];
        Console.WriteLine($"[Test] Trying to get transaction ID {firstTrans.TransactionID}");
        
        try
        {
            var detailed = controller.GetTransactionById(firstTrans.TransactionID);
            if (detailed != null)
            {
                Console.WriteLine($"[Test] SUCCESS! Got transaction: Type={detailed.Type}, Details={detailed.Details.Count}");
            }
            else
            {
                Console.WriteLine("[Test] FAILED: Got null result");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Test] FAILED with exception: {ex.Message}");
            Console.WriteLine($"[Test] StackTrace: {ex.StackTrace}");
        }
    }
    else
    {
        Console.WriteLine("[Test] No transactions found - create one first");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[Test] Error: {ex.Message}");
    Console.WriteLine($"[Test] StackTrace: {ex.StackTrace}");
}
