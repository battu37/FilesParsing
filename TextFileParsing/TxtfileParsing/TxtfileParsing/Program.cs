using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TxtfileParsing
{
    // /c/671d810b-c1b0-8013-8a99-73a24a9981c3
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
    }

    public class TxtFileParser
    {
        private readonly string filePath;

        public TxtFileParser(string path)
        {
            filePath = path;
        }

        // Read all lines and parse into a list of Employee objects
        public List<Employee> ReadAll()
        {
            var employees = new List<Employee>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"ID: (\d+) \| Name: (.+?) \| Age: (\d+) \| Department: (.+)");
                if (match.Success)
                {
                    employees.Add(new Employee
                    {
                        Id = int.Parse(match.Groups[1].Value),
                        Name = match.Groups[2].Value,
                        Age = int.Parse(match.Groups[3].Value),
                        Department = match.Groups[4].Value
                    });
                }
            }
            return employees;
        }


        // Add a new employee record to the file
        public void Add(Employee employee)
        {
            var line = $"ID: {employee.Id} | Name: {employee.Name} | Age: {employee.Age} | Department: {employee.Department}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        // Update an employee record by ID
        public void Update(int id, Employee updatedEmployee)
        {
            var lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith($"ID: {id}"))
                {
                    lines[i] = $"ID: {updatedEmployee.Id} | Name: {updatedEmployee.Name} | Age: {updatedEmployee.Age} | Department: {updatedEmployee.Department}";
                    File.WriteAllLines(filePath, lines);
                    return;
                }
            }
        }

        // Delete an employee record by ID
        public void Delete(int id)
        {
            var lines = new List<string>(File.ReadAllLines(filePath));
            lines.RemoveAll(line => line.StartsWith($"ID: {id}"));
            File.WriteAllLines(filePath, lines);
        }

    }

    public class Program
    {
        static void Main(string[] args)
        {
            var parser = new TxtFileParser(@"C:\Users\prave\OneDrive\Documents\Battu\Development\FilesParsing\TextFileParsing\TxtfileParsing\TxtfileParsing\Data");
            var employees = parser.ReadAll();
            parser.Add(new Employee { Id = 3, Name = "Alice", Age = 28, Department = "Finance" });
            parser.Update(2, new Employee { Id = 2, Name = "Jane Doe", Age = 26, Department = "HR" });
            parser.Delete(1);
        }
    }

}
