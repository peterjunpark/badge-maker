namespace Peter.BadgeMaker
{
    class Program
    {
        async static Task Main()
        {
            List<Employee> employees;
            Console.WriteLine("Welcome to BadgeMaker by Petercorp.\n");
            Console.WriteLine("Enter 1 to provide information to generate employee badges.");
            Console.WriteLine("Enter 2 to generate badges for random employees from the internet.\n");
            while (true)
            {
                string answer = Console.ReadLine() ?? "";
                if (answer == "1")
                {
                    employees = PeopleFetcher.GetEmployees();
                    break;
                }
                else if (answer == "2")
                {
                    employees = await PeopleFetcher.GetFromApi();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 1 or 2.");
                }
            }
            Util.PrintEmployees(employees);
            Util.MakeCSV(employees);
            await Util.MakeBadges(employees);
        }
    }
}
