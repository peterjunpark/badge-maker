namespace CatWorx.BadgeMaker
{
    class Program
    {
        public static void Main()
        {
            List<Employee> employees = GetEmployees();
            Util.PrintEmployees(employees);
            Util.MakeCSV(employees);
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

                Console.WriteLine("Enter employee id:");
                int id = int.Parse(Console.ReadLine() ?? "0");

                Console.WriteLine("Enter employee photo url:");
                string photoUrl = Console.ReadLine() ?? "";
                Employee employee = new(firstName, lastName, id, photoUrl);
                employees.Add(employee);
            }

            return employees;
        }
    }
}
