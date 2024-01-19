namespace CatWorx.BadgeMaker
{
    class Util
    {
        public static void PrintEmployees(List<Employee> employees)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                string template = "{0,-10}\t{1,-20}\t{2}";
                Console.WriteLine(
                    string.Format(
                        template,
                        employees[i].GetId(),
                        employees[i].GetFullName(),
                        employees[i].GetPhotoUrl()
                    )
                );
            }
        }

        public static void MakeCSV(List<Employee> employees)
        {
            if (!Directory.Exists("data"))
            {
                // create data direction if it doesn't exist
                Directory.CreateDirectory("data");
            }

            using (StreamWriter file = new("data/employees.csv")) // IDisposable
            {
                for (int i = 0; i < employees.Count; i++)
                {
                    file.WriteLine(
                        "{0},{1},{2}",
                        employees[i].GetId(),
                        employees[i].GetFullName(),
                        employees[i].GetPhotoUrl()
                    );
                }
            }
        }
    }
}
