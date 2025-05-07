using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading;

public static class JsonParser
{

    /// <summary>
    /// Метод для записи данных в формат JSON.
    /// </summary>
    /// <param name="followers">Подписчики для записи.</param>
    /// <param name="filePath">Путь к файлу для записи данных.</param>
    public static void WriteJson(FollowerCollection followers, string filePath)
    {
        try
        {
            // Проверяем доступность файла перед открытием.
            if (IsFileInUse(filePath))
            {
                Console.WriteLine($"Ошибка: файл '{filePath}' используется другим процессом.");
                return;
            }

            // Открываем файл для записи.
            using StreamWriter writer = new(filePath);

            // Сохраняем оригинальный поток вывода.
            TextWriter originalOut = Console.Out;

            // Перенаправляем Console.Out на файл.
            Console.SetOut(writer);

            // Записываем JSON в файл.
            Console.Out.WriteLine("{ \"elements\": [");

            for (int i = 0; i < followers.Followers.Count; i++)
            {
                Follower follower = followers.Followers[i];
                Console.Out.WriteLine("{");
                Console.Out.WriteLine($"\"id\": \"{follower.Id}\",");
                Console.Out.WriteLine($"\"label\": \"{follower.Label}\",");
                Console.Out.WriteLine("\"aspects\": {");

                foreach (KeyValuePair<string, int> aspect in follower.Aspects)
                {
                    Console.Out.WriteLine($"\"{aspect.Key}\": {aspect.Value},");
                }
                Console.Out.WriteLine("},");

                Console.Out.WriteLine($"\"description\": \"{follower.Description}\",");

                Console.Out.WriteLine("\"xtriggers\": {");
                foreach (KeyValuePair<string, string> trigger in follower.XTriggers)
                {
                    Console.Out.WriteLine($"\"{trigger.Key}\": \"{trigger.Value}\",");
                }
                Console.Out.WriteLine("},");

                Console.Out.WriteLine($"\"uniquenessgroup\": \"{follower.UniquenessGroup}\",");

                Console.Out.WriteLine($"\"animFrame\": \"{follower.AnimFrame}\",");
                Console.Out.WriteLine($"\"comments\": \"{follower.Comments}\",");
                Console.Out.WriteLine($"\"decayto\": \"{follower.Decayto}\",");
                Console.Out.WriteLine($"\"lifetime\": {follower.Lifetime},");
                Console.Out.WriteLine($"\"icon\": \"{follower.Icon}\"");

                Console.Out.WriteLine(i == followers.Followers.Count - 1 ? "}" : "},");
            }
            Console.Out.WriteLine("]}");
            // Возвращаем вывод обратно на стандартный поток.
            Console.SetOut(originalOut);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка при записи в файл: {ex.Message}");
        }
    }

    /// <summary>
    /// Метод для проверки, используется ли файл другим процессом.
    /// </summary>
    /// <param name="filePath">Путь до файла.</param>
    /// <returns>Занят ли файл.</returns>
    private static bool IsFileInUse(string filePath)
    {
        try
        {
            using FileStream fs = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            // Если файл открыт успешно, значит он не занят.
            return false;
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"Ошибка: Файл не найден. {ex.Message}\n");
            return true;
        }
        catch (DirectoryNotFoundException ex)
        {
            Console.WriteLine($"Ошибка: Директория не найдена. {ex.Message}\n");
            return true;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Ошибка ввода/вывода: {ex.Message}\n");
            return true;
        }
    }

    /// <summary>
    /// Метод для чтения данных из JSON.
    /// </summary>
    /// <param name="filePath">Путь до файла.</param>
    /// <returns>Коллекцию считанных подписчиков.</returns>
    public static FollowerCollection ReadJson(string filePath)
    {
        FollowerCollection collection = new();
        TextReader originalIn = Console.In;

        // Открываем файл для чтения.
        using StreamReader reader = new(filePath);
        // Перенаправляем Console.In на файл.
        Console.SetIn(reader);

        string json = Console.In.ReadToEnd();

        string pattern = @"\{\s*""id"":\s*""([^""]+)"",\s*""label"":\s*""([^""]+)"",\s*""aspects"":\s*\{([^}]+)\},\s*""description"":\s*""([^""]+)"",\s*""xtriggers"":\s*\{([^}]+)\},\s*""uniquenessgroup"":\s*""([^""]+)""(?:,\s*""animFrame"":\s*""([^""]+)"")?(?:,\s*""comments"":\s*""([^""]+)"")?(?:,\s*""decayto"":\s*""([^""]+)"")?(?:,\s*""lifetime"":\s*(\d+))?(?:,\s*""icon"":\s*""([^""]+)"")?\s*\}";

        MatchCollection matches = Regex.Matches(json, pattern, RegexOptions.Singleline);
        List<Thread> threads = []; // Список потоков.

        // Обрабатываем каждый элемент в отдельном потоке.
        foreach (Match match in matches)
        {
            Thread thread = new(() =>
            {
                string element = match.Value;
                string id = match.Groups[1].Value;
                string label = match.Groups[2].Value;
                string description = match.Groups[4].Value;
                string uniquenessGroup = match.Groups[7].Value;
                string animFrame = match.Groups[8].Value;
                string comments = match.Groups[9].Value;
                string decayto = match.Groups[10].Value;
                bool _ = int.TryParse(match.Groups[11].Value, out int lifetime);
                string icon = match.Groups[12].Value;

                // Парсим аспекты и триггеры.
                Dictionary<string, int> aspects = ParseAspects(match.Groups[3].Value);
                Dictionary<string, string> xTriggers = ParseXTriggers(match.Groups[5].Value);

                // Создаем объект Follower с новыми полями.
                Follower follower = new(id, label, aspects, description, xTriggers, uniquenessGroup, animFrame, comments, decayto, lifetime, icon);

                // Защищаем коллекцию от параллельного доступа.
                lock (collection)
                {
                    collection.AddFollower(follower);
                }
            });

            threads.Add(thread);
            thread.Start();
        }

        // Ожидаем завершения всех потоков.
        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        Console.SetIn(originalIn);
        return collection;
    }

    /// <summary>
    /// Вспомогательная функция для извлечения значений из JSON по шаблону.
    /// </summary>
    /// <param name="json">JSON из которого надо извлеч данные.</param>
    /// <param name="pattern">Паттерн по которому надо это сделать.</param>
    /// <returns></returns>
    private static string GetValueFromJson(string json, string pattern)
    {
        Match match = Regex.Match(json, pattern, RegexOptions.Singleline);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }


    /// <summary>
    /// Метод для парсинга аспектов.
    /// </summary>
    /// <param name="aspectsStr">JSON с аспектами.</param>
    /// <returns>Аспекты.</returns>
    private static Dictionary<string, int> ParseAspects(string aspectsStr)
    {
        Dictionary<string, int> aspects = [];

        aspectsStr = aspectsStr.Trim('{', '}', ' ');

        string[] pairs = aspectsStr.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (string pair in pairs)
        {
            string[] keyValue = pair.Split(':');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim().Trim('"');
                string value = keyValue[1].Trim();

                if (int.TryParse(value, out int intValue))
                {
                    aspects[key] = intValue;
                }
            }
        }
        return aspects;
    }

    /// <summary>
    /// Метод для парсинга триггеров.
    /// </summary>
    /// <param name="xTriggersStr">JSON с триггерами.</param>
    /// <returns>Триггеры</returns>
    /// <exception cref="ArgumentNullException">При неправильном JSON.</exception>
    private static Dictionary<string, string> ParseXTriggers(string xTriggersStr)
    {
        Dictionary<string, string> xTriggers = new();

        if (string.IsNullOrWhiteSpace(xTriggersStr)) return xTriggers;

        xTriggersStr = xTriggersStr.Trim('{', '}', ' ');

        string[] pairs = xTriggersStr.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (string pair in pairs)
        {
            string[] keyValue = pair.Split(':');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0].Trim().Trim('"');
                string value = keyValue[1].Trim().Trim('"');

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    xTriggers[key] = value;
                }
            }
        }
        return xTriggers;
    }

}
