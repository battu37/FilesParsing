using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace LogFileParsing
{
    public class LogEntry
    {
        public string Level { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }

    public class LogFileParser
    {
        private readonly string filePath;

        public LogFileParser(string path)
        {
            filePath = path;
        }

        // Read and parse log entries
        public List<LogEntry> ReadAll()
        {
            var logs = new List<LogEntry>();
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"\[(\w+)\] (\d+-\d+-\d+ \d+:\d+:\d+) (.+)");
                if (match.Success)
                {
                    logs.Add(new LogEntry
                    {
                        Level = match.Groups[1].Value,
                        Timestamp = DateTime.Parse(match.Groups[2].Value),
                        Message = match.Groups[3].Value
                    });
                }
            }
            return logs;
        }

        // Add a new log entry
        public void Add(LogEntry log)
        {
            var line = $"[{log.Level}] {log.Timestamp:yyyy-MM-dd HH:mm:ss} {log.Message}";
            File.AppendAllText(filePath, line + Environment.NewLine);
        }

        // Update a log entry by searching for a timestamp
        public void Update(DateTime timestamp, string newMessage)
        {
            var lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(timestamp.ToString("yyyy-MM-dd HH:mm:ss")))
                {
                    var parts = lines[i].Split(' ');
                    lines[i] = $"{parts[0]} {parts[1]} {parts[2]} {newMessage}";
                    File.WriteAllLines(filePath, lines);
                    return;
                }
            }
        }

        // Delete log entries based on a condition
        public void Delete(string level)
        {
            var lines = new List<string>(File.ReadAllLines(filePath));
            lines.RemoveAll(line => line.Contains($"[{level}]"));
            File.WriteAllLines(filePath, lines);
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var logParser = new LogFileParser("app.log");
            var logs = logParser.ReadAll();
            logParser.Add(new LogEntry { Level = "INFO", Timestamp = DateTime.Now, Message = "Application started" });
            logParser.Update(DateTime.Now.AddMinutes(-5), "User login successful");
            logParser.Delete("ERROR");
        }
    }

}






