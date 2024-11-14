using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace TxtfileParsing
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Department { get; set; }
    }

    public class CsvFileParser
    {
        private readonly string filePath;
        private readonly CsvConfiguration config;

        public CsvFileParser(string path)
        {
            filePath = path;
            config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };
        }

        // Read all entries in the CSV file
        public List<Employee> ReadAll()
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            return csv.GetRecords<Employee>().ToList();
        }

        // Add a new record
        public void Add(Employee employee)
        {
            var employees = ReadAll();
            employees.Add(employee);
            WriteAll(employees);
        }

        // Update a record by ID
        public void Update(int id, Employee updatedEmployee)
        {
            var employees = ReadAll();
            var employee = employees.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                employee.Name = updatedEmployee.Name;
                employee.Age = updatedEmployee.Age;
                employee.Department = updatedEmployee.Department;
                WriteAll(employees);
            }
        }

        // Delete a record by ID
        public void Delete(int id)
        {
            var employees = ReadAll();
            employees.RemoveAll(e => e.Id == id);
            WriteAll(employees);
        }

        // Helper to write all data back to the file
        private void WriteAll(List<Employee> employees)
        {
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, config);
            csv.WriteRecords(employees);
        }

    }

    public class Program
    {
        static void Main(string[] args)
        {
            var parser = new CsvFileParser(@"C:\Users\prave\OneDrive\Documents\Battu\Development\FilesParsing\TextFileParsing\TxtfileParsing\TxtfileParsing\Data");
            var employees = parser.ReadAll();
            /*parser.Add(new Employee { Id = 3, Name = "Alice", Age = 28, Department = "Finance" });
            parser.Update(2, new Employee { Id = 2, Name = "Jane Doe", Age = 26, Department = "HR" });
            parser.Delete(1);*/
        }
    }


}
