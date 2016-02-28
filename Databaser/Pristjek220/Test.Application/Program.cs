using System;
using System.ComponentModel;
using System.Linq;
using Database;

namespace Test.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Context())
            {
                string pname;
                string sname;
                double newPrice;
                Storemanager aldi, føtex, fakta;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("What action do want to do?");
                    Console.WriteLine("1. Add a product to the database");
                    Console.WriteLine("2. Add a storeManager to the database");
                    Console.WriteLine("3. Add a storeProduct to the database");
                    Console.WriteLine("4. Remove a product from the database");
                    Console.WriteLine("5. Remove a store from the database");
                    Console.WriteLine("6. Remove a storeproduct from the database");
                    Console.WriteLine("7. Chance the price of a product");
                    Console.WriteLine("8. Find a product in the database");
                    Console.WriteLine("9. Find a store in the database");
                    Console.WriteLine("10. Find a storeproduct in the database");
                    Console.WriteLine("11. Find the cheapest store");

                    var action = int.Parse(Console.ReadLine());

                    switch (action)
                    {
                        case (1):
                            Console.Clear();
                            Console.Write("Enter the name of the product you want to add: ");
                            pname = Console.ReadLine();
                            Console.Write("Enter the price of the product: ");
                            newPrice = double.Parse(Console.ReadLine());

                            if(db.AddProductToDatabase(pname) != 0)
                                Console.WriteLine($"{pname} already exists in");
                            break;
                        case (2):
                            Console.Clear();
                            Console.Write("Enter the name of the store you want to add: ");
                            sname = Console.ReadLine();

                            db.AddStoreToDatabase(sname);
                            break;
                        case (3):
                            Console.Clear();
                            Console.Write("Enter the name of the Product you want to connect to a store: ");
                            pname = Console.ReadLine();
                            Console.Write("Enter the name of the Store you want to connect to a product: ");
                            sname = Console.ReadLine();
                            Console.Write($"Enter the price of {pname} in {sname}: ");
                            newPrice = double.Parse(Console.ReadLine());

                            db.AddStoreProductRelationToDatabase(pname, sname, newPrice);
                            break;
                        case (4):
                            Console.Clear();
                            Console.Write("Enter the name of the product you want to remove: ");
                            pname = Console.ReadLine();

                            db.RemoveProductFromDatabase(pname);
                            break;
                        case (5):
                            Console.Clear();
                            Console.Write("Enter the name of the store you want to remove: ");
                            sname = Console.ReadLine();

                            db.RemoveStoreFromDatabse(sname);
                            break;
                        case (6):
                            Console.Clear();
                            Console.Write("Enter the name of the product you want to remove: ");
                            pname = Console.ReadLine();
                            Console.Write("Enter the name of the store you want to remove: ");
                            sname = Console.ReadLine();

                            db.RemoveStoreProductFromDatabse(sname, pname);
                            break;
                        case (7):
                            Console.Clear();
                            Console.Write("Enter the name of the Product: ");
                            pname = Console.ReadLine();
                            Console.Write("Enter the name of the Store: ");
                            sname = Console.ReadLine();
                            Console.Write($"Enter the new price for {pname} in {sname}: ");
                            newPrice = Double.Parse(Console.ReadLine());

                            db.ChangePriceOfProductInAStore(pname, sname, newPrice);

                            break;
                        /* case (8):
                             Console.Clear();
                             Console.Write("Enter the name of the Product: ");
                             pname = Console.ReadLine();

                             Console.WriteLine($"Product found is: {db.FindProduct(pname).ProductName}");
                             break;
                         case (9):
                             Console.Clear();
                             Console.Write("Enter the name of the Store: ");
                             sname = Console.ReadLine();

                             Console.WriteLine($"Store found is: {db.FindStore(sname).StoreName}");
                             break;
                         case (10):
                             Console.Clear();
                             Console.Write("Enter the name of the Product: ");
                             pname = Console.ReadLine();
                             Console.Write("Enter the name of the Store: ");
                             sname = Console.ReadLine();

                             Console.WriteLine($"Prisen for {db.FindStoreProduct(pname, sname).Product.ProductName} in {db.FindStoreProduct(pname, sname).Store.StoreName} is: {db.FindStoreProduct(pname, sname).Price}");
                             break;*/
                         case (11):
                             Console.Clear();
                             Console.Write("Enter the name of the Product: ");
                             pname = Console.ReadLine();

                             var prod = db.FindProduct(pname);
                            StoreProduct storprod = prod.StoreProducts.FirstOrDefault();

                            foreach (var storeprod in prod.StoreProducts)
                            {
                                if (storeprod.Price < storprod.Price)
                                {
                                    storprod = storeprod;
                                }
                            }

                             Console.WriteLine($"The cheapest store for {pname} is: {db.Stores.Find(storprod.StoreId).StoreName}");
                             break;
                        default:
                            Console.Clear();
                            Console.WriteLine("You have not entered a valid action");
                            break;
                    }

                    Console.WriteLine("Hit Enter to continue.");
                    Console.ReadLine();
                }
            }
        }
    }
}
