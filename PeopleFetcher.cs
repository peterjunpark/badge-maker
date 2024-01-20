using System.Security.Principal;
using Newtonsoft.Json.Linq;

namespace Peter.BadgeMaker
{
  class PeopleFetcher
  {
    async public static Task<List<Employee>> GetFromApi()
    {
      List<Employee> employees = new();
      using (HttpClient client = new())
      {
        string res = await client.GetStringAsync("https://randomuser.me/api/?results=10&nat=us&inc=name,id,picture");
        JObject json = JObject.Parse(res);
        JToken? results = json.SelectToken("results");
        if (results != null)
        {
          foreach (JToken employeeData in results)
          {
            string firstName = employeeData.SelectToken("name.first")?.ToString() ?? "";
            string lastName = employeeData.SelectToken("name.last")?.ToString() ?? "";
            string idValue = employeeData.SelectToken("id.value")?.ToString() ?? "";
            int id = int.Parse(idValue.Replace("-", ""));
            string pictureUrl = employeeData.SelectToken("picture.large")?.ToString() ?? "http://placekitten.com/400/400";

            Employee employee = new(firstName, lastName, id, pictureUrl);
            employees.Add(employee);
          }
        }
      }
      return employees;
    }
    public static List<Employee> GetEmployees()
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
