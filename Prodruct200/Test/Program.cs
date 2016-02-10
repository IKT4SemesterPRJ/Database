using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prodruct200;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new Product220Context())
            {
                string pname;
                string sname;
                double newPrice;

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("What action do want to do?");
                    Console.WriteLine("1. Add a product to the database");
                    Console.WriteLine("2. Remove a product to the database");
                    Console.WriteLine("3. Chance the price of a product");
                    Console.WriteLine("4. Find a product in the database");
                    Console.WriteLine("5. Find the cheapest store");
                    Console.WriteLine("6. Add a store to a product");

                    var action = int.Parse(Console.ReadLine());

                    switch (action)
                    {
                        case (1):
                            Console.Clear();
                            db.AddProduct();
                            break;
                        case (2):
                            Console.Clear();
                            Console.Write("Enter the name of the product you want to remove: ");
                            pname = Console.ReadLine();

                            db.RemoveProductdatabase(pname);
                            break;
                        case (3):
                            Console.Clear();
                            Console.Write("Enter the name of the Product: ");
                            pname = Console.ReadLine();
                            Console.Write("Enter the name of the Store: ");
                            sname = Console.ReadLine();
                            Console.Write($"Enter the new price for {pname} in {sname}: ");
                            newPrice = Double.Parse(Console.ReadLine());

                            db.ChangePriceOfProduct(pname, sname, newPrice);

                            break;
                        case (4):
                            Console.Clear();
                            Console.Write("Enter the name of the Product: ");
                            pname = Console.ReadLine();
                            Console.Write("Enter the name of the Store: ");
                            sname = Console.ReadLine();

                            Console.WriteLine($"Prisen for {pname} in {sname} is: {db.FindPrice(pname, sname)}");
                            break;
                        case (5):
                            Console.Clear();
                            Console.Write("Enter the name of the Product: ");
                            pname = Console.ReadLine();

                            Console.WriteLine($"The cheapest store for {pname} is: {db.CheapestStore(pname)}");
                            break;
                        case (6):
                            Console.Write("Enter the name of the product: ");
                            pname = Console.ReadLine();
                            Console.Write($"Enter the name of the store you want to add to {pname}: ");
                            sname = Console.ReadLine();
                            Console.Write($"Enter the prise for {pname} in {sname}: ");
                            newPrice = double.Parse(Console.ReadLine());

                            db.AddStoreToProduct(pname, sname, newPrice);
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
