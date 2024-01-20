namespace Peter.BadgeMaker
{
    class Program
    {
        async static Task Main()
        {
            List<Employee> employees = GetEmployees();
            Util.PrintEmployees(employees);
            Util.MakeCSV(employees);
            await Util.MakeBadges(employees);
        }

        private static List<Employee> GetEmployees()
        {
            List<Employee> employees = new();

            while (true)
            {
                Console.WriteLine("Enter first name (leave empty to exit):");
                string firstName = Console.ReadLine() ?? "";

                if (firstName == "")
                {
                    break;
                }

                Console.WriteLine("Enter last name:");
                string lastName = Console.ReadLine() ?? "";

                int id;
                while (true)
                {
                    Console.WriteLine("Enter employee id:");
                    if (int.TryParse(Console.ReadLine(), out id))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer for the employee id.");
                    }
                }

                Console.WriteLine("Enter employee photo url (leave empty for placeholder):");
                string photoUrl = Console.ReadLine() ?? "";
                photoUrl = string.IsNullOrEmpty(photoUrl) ? "http://placekitten.com/400/400" : photoUrl;
                Employee employee = new(firstName, lastName, id, photoUrl);
                employees.Add(employee);

                Console.WriteLine("Make another employee badge? (y/n):");
                string answer = Console.ReadLine() ?? "";
                if (answer.ToLower() != "y")
                {
                    break;
                }
            }

            return employees;
        }
    }
}
