using System;
using System.Threading.Tasks;

namespace make_autosite
{
    class Program
    {
        static async Task Main(string[] args)
        {

            switch (args.Length)
            {
                case 0:
                    Console.WriteLine("Please provide site name");
                    break;
                case 1:
                    await new WebGenerator(args[0]) { }.GenerateAsync();
                    break;
                default:
                    await new WebGenerator(args[0]) { }.GenerateAsync();
                    break;
            }

        }

    }
}
