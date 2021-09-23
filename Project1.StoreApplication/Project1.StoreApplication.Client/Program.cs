using System;

using Project1.StoreApplication.Storage;

namespace Project1.StoreApplication.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var stor = new DBStorageImpl();
            var cs = stor.GetModelCustomers();
            foreach (var c in cs)
            {
                Console.WriteLine(c);
            }
        }
    }
}
