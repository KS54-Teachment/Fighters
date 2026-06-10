using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace AppealManager
{
    public class Appeal
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsProcessed { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {UserName} - {Message} (Создано: {CreatedAt}, Обработано: {IsProcessed})";
        }
    }

    public interface IAppealStorage
    {
        void SaveAppeals(List<Appeal> appeals);
        List<Appeal> LoadAppeals();
    }

    public class CsvAppealStorage : IAppealStorage
    {
        private readonly string _filePath;

        public CsvAppealStorage(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveAppeals(List<Appeal> appeals)
        {
            List<string> lines = new List<string>();
            lines.Add("Id,UserName,Message,CreatedAt,IsProcessed");

            foreach (Appeal appeal in appeals)
            {
                string escapedMessage = appeal.Message;
                if (escapedMessage.Contains(","))
                {
                    escapedMessage = "\"" + escapedMessage + "\"";
                }
                if (escapedMessage.Contains("\""))
                {
                    escapedMessage = escapedMessage.Replace("\"", "\"\"");
                }

                lines.Add(string.Format("{0},{1},{2},{3},{4}",
                    appeal.Id,
                    appeal.UserName,
                    escapedMessage,
                    appeal.CreatedAt.ToString("O"),
                    appeal.IsProcessed));
            }

            File.WriteAllLines(_filePath, lines, Encoding.UTF8);
        }

        public List<Appeal> LoadAppeals()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Appeal>();
            }

            List<Appeal> appeals = new List<Appeal>();
            string[] lines = File.ReadAllLines(_filePath, Encoding.UTF8);

            if (lines.Length <= 1)
            {
                return appeals;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = ParseCsvLine(lines[i]);
                if (parts.Length >= 5)
                {
                    Appeal appeal = new Appeal();
                    appeal.Id = int.Parse(parts[0]);
                    appeal.UserName = parts[1];
                    appeal.Message = parts[2].Trim('"');
                    appeal.CreatedAt = DateTime.Parse(parts[3], null, DateTimeStyles.RoundtripKind);
                    appeal.IsProcessed = bool.Parse(parts[4]);
                    appeals.Add(appeal);
                }
            }

            return appeals;
        }

        private string[] ParseCsvLine(string line)
        {
            List<string> result = new List<string>();
            bool inQuotes = false;
            StringBuilder current = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (line[i] == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(line[i]);
                }
            }
            result.Add(current.ToString());

            return result.ToArray();
        }
    }

    public class TxtAppealStorage : IAppealStorage
    {
        private readonly string _filePath;

        public TxtAppealStorage(string filePath)
        {
            _filePath = filePath;
        }
        public void SaveAppeals(List<Appeal> appeals)
        {
            List<string> lines = new List<string>();
            lines.Add("# Appeals storage - " + DateTime.Now.ToString());
            lines.Add("# Total appeals: " + appeals.Count);
            lines.Add("# Format: Id|UserName|Message|CreatedAt|IsProcessed");

            foreach (Appeal appeal in appeals)
            {
                string escapedUserName = EscapeMessage(appeal.UserName);
                string escapedMessage = EscapeMessage(appeal.Message);
                lines.Add(string.Format("{0}|{1}|{2}|{3}|{4}",
                    appeal.Id,
                    escapedUserName,
                    escapedMessage,
                    appeal.CreatedAt.ToString("O"),
                    appeal.IsProcessed));
            }

            File.WriteAllLines(_filePath, lines, Encoding.UTF8);
        }

        public List<Appeal> LoadAppeals()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Appeal>();
            }

            List<Appeal> appeals = new List<Appeal>();
            string[] lines = File.ReadAllLines(_filePath, Encoding.UTF8);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                {
                    continue;
                }

                string[] parts = line.Split('|');
                if (parts.Length >= 5)
                {
                    Appeal appeal = new Appeal();
                    appeal.Id = int.Parse(parts[0]);
                    appeal.UserName = UnescapeMessage(parts[1]);
                    appeal.Message = UnescapeMessage(parts[2]);
                    appeal.CreatedAt = DateTime.Parse(parts[3], null, DateTimeStyles.RoundtripKind);
                    appeal.IsProcessed = bool.Parse(parts[4]);
                    appeals.Add(appeal);
                }
            }

            return appeals;
        }

        private string EscapeMessage(string msg)
        {
            string result = msg;
            result = result.Replace("\n", "\\n");
            result = result.Replace("\r", "\\r");
            result = result.Replace("|", "\\|");
            return result;
        }

        private string UnescapeMessage(string msg)
        {
            string result = msg;
            result = result.Replace("\\n", "\n");
            result = result.Replace("\\r", "\r");
            result = result.Replace("\\|", "|");
            return result;
        }
    }

    class Program
    {
        private static List<Appeal> _appeals = new List<Appeal>();
        private static int _nextId = 1;
        private static IAppealStorage _storage;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Выберите формат хранения:");
            Console.WriteLine("1 - CSV");
            Console.WriteLine("2 - TXT");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                _storage = new CsvAppealStorage("appeals.csv");
            }
            else
            {
                _storage = new TxtAppealStorage("appeals.txt");
            }

            LoadData();

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("\n=== Система обращений ===");
                Console.WriteLine("1. Добавить обращение");
                Console.WriteLine("2. Показать все обращения");
                Console.WriteLine("3. Обработать обращение");
                Console.WriteLine("4. Сохранить и выйти");
                Console.WriteLine("5. Сохранить и продолжить");
                Console.Write("Выберите действие: ");

                string key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        AddAppeal();
                        break;
                    case "2":
                        ShowAllAppeals();
                        break;
                    case "3":
                        ProcessAppeal();
                        break;
                    case "4":
                        SaveData();
                        running = false;
                        break;
                    case "5":
                        SaveData();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }

                if (running && key != "5")
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Программа завершена.");
        }

        static void LoadData()
        {
            _appeals = _storage.LoadAppeals();
            if (_appeals.Any())
            {
                _nextId = _appeals.Max(a => a.Id) + 1;
            }
            Console.WriteLine($"Загружено {_appeals.Count} обращений.");
            Thread.Sleep(1500);
        }

        static void SaveData()
        {
            _storage.SaveAppeals(_appeals);
            Console.WriteLine($"Сохранено {_appeals.Count} обращений.");
            Thread.Sleep(1000);
        }

        static void AddAppeal()
        {
            Console.Write("Введите имя пользователя: ");
            string userName = Console.ReadLine();

            Console.Write("Введите текст обращения: ");
            string message = Console.ReadLine();

            Appeal appeal = new Appeal();
            appeal.Id = _nextId++;
            appeal.UserName = userName;
            appeal.Message = message;
            appeal.CreatedAt = DateTime.Now;
            appeal.IsProcessed = false;

            _appeals.Add(appeal);
            Console.WriteLine($"Обращение #{appeal.Id} добавлено!");
        }

        static void ShowAllAppeals()
        {
            if (!_appeals.Any())
            {
                Console.WriteLine("Нет обращений.");
                return;
            }

            Console.WriteLine($"\nВсего обращений: {_appeals.Count}\n");
            foreach (Appeal appeal in _appeals)
            {
                Console.WriteLine(appeal.ToString());
                Console.WriteLine(new string('-', 50));
            }
        }

        static void ProcessAppeal()
        {
            Console.Write("Введите ID обращения для обработки: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Appeal appeal = _appeals.FirstOrDefault(a => a.Id == id);
                if (appeal != null)
                {
                    appeal.IsProcessed = true;
                    Console.WriteLine($"Обращение #{id} отмечено как обработанное.");
                }
                else
                {
                    Console.WriteLine("Обращение не найдено.");
                }
            }
            else
            {
                Console.WriteLine("Неверный ID.");
            }
        }
    }
}