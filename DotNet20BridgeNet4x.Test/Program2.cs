using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Concurrency
{
    public interface IExample
    {
        int Id { get; }
        Task RunAsync();
    }

    public static class Program
    {
        public static void Main()
        {
            //Example.PrintWithThread("Started main method.");

            var examples = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.GetInterface(nameof(IExample)) != null
                    && !type.IsAbstract)
                .Select(type => (IExample)Activator.CreateInstance(type))
                .ToDictionary(example => example.Id.ToString());

            var id = Console.ReadLine();
            if (!examples.ContainsKey(id))
            {
                Console.WriteLine("No such example.");
                return;
            }

            var example = examples[id];
            example.RunAsync().Wait();

            //Example.PrintWithThread("Finished main method.");
            Console.ReadLine();
        }
    }
}
