using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consumer;
using Pristjek220Data;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IUnitOfWork unit = new UnitOfWork(new DataContext());
            IConsumer cons = new Consumer.Consumer(unit);

            List<Product> list = new List<Product>();

            list.Add(new Product() {ProductName = "Tomat"});
            list.Add(new Product() { ProductName = "Agurk" });
            list.Add(new Product() { ProductName = "Banan" });

            var ting = cons.FindCheapestStoreWithSumForListOfProducts(list);

            Console.WriteLine($"{ting.Name}  {ting.Price}");
        }
    }
}
