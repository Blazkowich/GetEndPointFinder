using EndPointFinder.Repository.UnitOfWork;

namespace EndPointFinder
{
    public class Program
    {
        private static readonly IUnitOfWork _unitOfWork = new UnitOfWork();

        static async Task Main(string[] args)
        {
            do
            {
                Console.WriteLine("Please Enter Number Between 1 - 9:");
                Console.WriteLine("1. Scan Web Site For Find Endpoints");
                Console.WriteLine();
                Console.WriteLine("2. Scan Web Site For Find Api's");
                Console.WriteLine();
                Console.WriteLine("3. Exit");
                Console.WriteLine();

                int input;
                if (!int.TryParse(Console.ReadLine(), out input))
                {
                    Console.WriteLine("Invalid Input. Please enter an integer.");
                    continue;
                }

                switch (input)
                {
                    case 1:
                        await _unitOfWork.MainMethods.ScanWebSiteForEnpoints(_unitOfWork.HelperMethods.GetValidUrl());
                        break;
                    case 2:
                        _unitOfWork.MainMethods.ScanWebSiteForApis(_unitOfWork.HelperMethods.GetValidUrl());
                        break;
                    case 3:
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            } while (true);
        }
    }
}
