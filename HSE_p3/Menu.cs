using System.IO;
using System.Security;

public class Menu
{
    private FollowerCollection followers = new();

    /// <summary>
    /// Отображает меню и выполняет выбранную операцию.
    /// </summary>
    public void ShowMenu()
    {
        while (true)
        {

            Console.WriteLine("1. Ввести данные (консоль/файл)");
            Console.WriteLine("2. Отфильтровать данные");
            Console.WriteLine("3. Отсортировать данные");
            Console.WriteLine("4. Построить дерево последователей");
            Console.WriteLine("5. Визуализировать дерево последователей");
            Console.WriteLine("6. Вывести данные (консоль/файл)");
            Console.WriteLine("7. Выход");

            string? choice = Console.ReadLine();
            try
            {
                switch (choice)
                {
                    case "1":
                        ReadData(); // Загружает данные.
                        break;
                    case "2":
                        FilterData(); // Фильтрует данные.
                        break;
                    case "3":
                        SortData(); // Сортирует данные.
                        break;
                    case "4":
                        BuildFollowerTree(); // Строит дерево последователей.
                        break;
                    case "5":
                        FollowerTreeVisualizer visualizer = new(followers);
                        visualizer.VisualizeFollowerTree(); // Визуализирует дерево.
                        break;
                    case "6":
                        WriteData(); // Записывает данные.
                        break;
                    case "7":
                        return; // Выход из программы.
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите вариант из меню.");
                        break;
                }
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Ошибка: Параметр не может быть null. {ex.Message}\n");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Ошибка: Значение выходит за допустимые границы. {ex.Message}\n");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: Некорректный путь или аргумент. {ex.Message}\n");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Ошибка: Операция не может быть выполнена в текущем состоянии. {ex.Message}\n");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Ошибка формата: {ex.Message}\n");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Ошибка: Деление на ноль. {ex.Message}\n");
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine($"Ошибка: Файл не найден. {ex.Message}\n");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.Error.WriteLine($"Ошибка: Директория не найдена. {ex.Message}\n");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода/вывода: {ex.Message}\n");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.Error.WriteLine($"Ошибка доступа: {ex.Message}\n");
            }
            catch (SecurityException ex)
            {
                Console.WriteLine($"Ошибка безопасности: {ex.Message}\n");
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine($"Ошибка: Недостаточно памяти. {ex.Message}\n");
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Ошибка: Индекс вне диапазона. {ex.Message}\n");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"Ошибка: Ссылка на null. {ex.Message}\n");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Ошибка: Превышено время ожидания. {ex.Message}\n");
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine($"Ошибка: Некорректное преобразование типа. {ex.Message}\n");
            }
            catch (OverflowException ex)
            {
                Console.WriteLine($"Ошибка: Переполнение числового значения. {ex.Message}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}\n");
            }
        }
    }

    /// <summary>
    /// Читает данных.
    /// </summary>
    private void ReadData()
    {
        Console.WriteLine("Выберите источник данных (1 - консоль, иначе - файл):");
        string? source = Console.ReadLine();
        try
        {
            if (source?.ToLower() != "1")
            {
                // Загрузка данных из файла.
                Console.WriteLine("Введите путь к файлу:");
                string? path = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(path))
                {
                    Console.WriteLine("Ошибка: путь не может быть пустым.");
                    return;
                }

                followers = JsonParser.ReadJson(path);
                Console.WriteLine("Данные успешно загружены.");
            }
            else
            {
                // Ввод данных вручную.
                Console.WriteLine("Данные будут введены вручную.");

                Console.WriteLine("Введите количество последователей для добавления:");
                if (int.TryParse(Console.ReadLine(), out int followerCount) && followerCount > 0)
                {
                    for (int i = 0; i < followerCount; i++)
                    {
                        Console.WriteLine($"Ввод данных для последователя {i + 1}:");

                        // Ввод данных для последователя.
                        Console.WriteLine("Введите ID последователя:");
                        string? id = Console.ReadLine()?.Trim();

                        Console.WriteLine("Введите метку последователя (label):");
                        string? label = Console.ReadLine()?.Trim();

                        Console.WriteLine("Введите описание последователя (description):");
                        string? description = Console.ReadLine()?.Trim();

                        Console.WriteLine("Введите уникальную группу (uniquenessgroup):");
                        string? uniquenessGroup = Console.ReadLine()?.Trim();

                        // Ввод аспекта.
                        Dictionary<string, int> aspects = [];
                        Console.WriteLine("Введите аспекты (формат: ключ:значение), введите пустую строку для завершения:");
                        while (true)
                        {
                            Console.WriteLine("Аспект (ключ:значение):");
                            string aspect = Console.ReadLine()?.Trim();
                            if (string.IsNullOrWhiteSpace(aspect))
                            {
                                break;
                            }

                            string[] aspectParts = aspect.Split(':');
                            if (aspectParts.Length == 2 && int.TryParse(aspectParts[1], out int aspectValue))
                            {
                                aspects[aspectParts[0].Trim()] = aspectValue;
                            }
                            else
                            {
                                Console.WriteLine("Неверный формат аспекта, попробуйте еще раз.");
                            }
                        }

                        // Ввод триггеров.
                        Dictionary<string, string> xTriggers = [];
                        Console.WriteLine("Введите триггеры (формат: ключ:значение), введите пустую строку для завершения:");
                        while (true)
                        {
                            Console.WriteLine("Триггер (ключ:значение):");
                            string trigger = Console.ReadLine()?.Trim();
                            if (string.IsNullOrWhiteSpace(trigger))
                            {
                                break;
                            }

                            string[] triggerParts = trigger.Split(':');
                            if (triggerParts.Length == 2)
                            {
                                xTriggers[triggerParts[0].Trim()] = triggerParts[1].Trim();
                            }
                            else
                            {
                                Console.WriteLine("Неверный формат триггера, попробуйте еще раз.");
                            }
                        }

                        Console.WriteLine("Введите анимационный кадр (animFrame):");
                        string animFrame = Console.ReadLine()?.Trim();

                        Console.WriteLine("Введите комментарии (comments):");
                        string comments = Console.ReadLine()?.Trim();

                        Console.WriteLine("Введите следующее состояние (decayto):");
                        string decayto = Console.ReadLine()?.Trim();

                        Console.WriteLine("Введите время жизни (lifetime):");
                        int lifetime = int.TryParse(Console.ReadLine(), out int result) ? result : 0;

                        Console.WriteLine("Введите иконку (icon):");
                        string icon = Console.ReadLine()?.Trim();

                        followers.AddFollower(new Follower(
                            id, label, aspects, description, xTriggers, uniquenessGroup,
                            animFrame, comments, decayto, lifetime, icon));

                        Console.WriteLine($"Последователь {label} успешно добавлен.");
                    }
                    Console.WriteLine("Все данные успешно добавлены.");
                }
                else
                {
                    Console.WriteLine("Ошибка: количество последователей должно быть числом больше 0.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
        }
    }



    /// <summary>
    /// Фильтрует данные на основе выбранных полей.
    /// </summary>
    private void FilterData()
    {
        Console.WriteLine("Выберите поля для фильтрации из доступных полей (можно выбрать несколько через запятую):");
        IEnumerable<string> availableFields = followers.Followers[0].GetAllFields();
        for (int i = 0; i < availableFields.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. {availableFields.ElementAt(i)}");
        }

        Console.WriteLine("Введите номера полей для фильтрации (через запятую):");
        string input = Console.ReadLine();
        List<int> fieldIndexes = []; 

        if (input != null)
        {
            string[] inputs = input.Split(',');

            foreach (string index in inputs)
            {
                if (int.TryParse(index.Trim(), out int idx) && idx > 0 && idx <= availableFields.Count())
                {
                    fieldIndexes.Add(idx - 1);
                }
            }
        }

        if (fieldIndexes.Count == 0)
        {
            Console.WriteLine("Неверный выбор полей для фильтрации.");
            return;
        }

        // Собираем список выбранных полей.
        List<string> selectedFields = []; 
        foreach (int idx in fieldIndexes)
        {
            selectedFields.Add(availableFields.ElementAt(idx));
        }

        Console.WriteLine("Вы выбрали поля: " + string.Join(", ", selectedFields));

        // Запрашиваем критерии фильтрации для каждого поля.
        Dictionary<string, string> fieldFilters = []; 
        foreach (string field in selectedFields)
        {
            Console.WriteLine($"Введите критерий фильтрации для поля {field} (оставьте пустым, чтобы не фильтровать):");
            string filterValue = Console.ReadLine();
            if (!string.IsNullOrEmpty(filterValue))
            {
                fieldFilters.Add(field, filterValue.Trim());
            }
        }

        // Применяем фильтрацию.
        List<Follower> filteredFollowers = [];
        foreach (Follower follower in followers.Followers)
        {
            bool matches = true;

            foreach (string field in selectedFields)
            {
                string fieldValue = follower.GetField(field);

                // Проверяем, если поле отфильтровано, и значение пустое, то не подходит
                if (fieldFilters.ContainsKey(field) && !string.IsNullOrEmpty(fieldValue) && !fieldValue.Contains(fieldFilters[field]))
                {
                    matches = false;
                    break;
                }

                // Для коллекций Aspects и XTriggers проверяем, что они не пустые.
                if ((field == "Aspects" && follower.Aspects.Count == 0) ||
                    (field == "XTriggers" && follower.XTriggers.Count == 0))
                {
                    matches = false;
                    break;
                }
            }

            if (matches)
            {
                filteredFollowers.Add(follower);
            }
        }

        // Отображаем отфильтрованные данные.
        DisplayFollowers(new FollowerCollection(filteredFollowers));
    }


    /// <summary>
    /// Сортирует данные по выбранному полю.
    /// </summary>
    private void SortData()
    {
        Console.WriteLine("Выберите поле для сортировки из доступных полей:");
        IEnumerable<string> availableFields = followers.Followers[0].GetAllFields();
        for (int i = 0; i < availableFields.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. {availableFields.ElementAt(i)}");
        }

        Console.WriteLine("Введите номер поля для сортировки:");
        if (int.TryParse(Console.ReadLine(), out int fieldIndex) && fieldIndex > 0 && fieldIndex <= availableFields.Count())
        {
            string fieldName = availableFields.ElementAt(fieldIndex - 1);
            Console.WriteLine($"Вы выбрали поле: {fieldName}");

            Console.WriteLine("Выберите направление сортировки (1 - по возрастанию, 2 - по убыванию):");
            string sortDirection = Console.ReadLine();

            if (sortDirection == "1")
            {
                List<Follower> sortedFollowers = [.. followers.Followers.OrderBy(f => f.GetField(fieldName))];
                DisplayFollowers(new FollowerCollection(sortedFollowers));
            }
            else if (sortDirection == "2")
            {
                List<Follower> sortedFollowers = [.. followers.Followers.OrderByDescending(f => f.GetField(fieldName))];
                DisplayFollowers(new FollowerCollection(sortedFollowers));
            }
            else
            {
                Console.WriteLine("Неверный выбор направления сортировки.");
            }
        }
        else
        {
            Console.WriteLine("Неверный номер поля.");
        }
    }

    /// <summary>
    /// Отображает данные в консоль.
    /// </summary>
    /// <param name="followerToDisplay">Данные о подписчиках.</param>
    private void DisplayFollowers(FollowerCollection followerToDisplay)
    {
        foreach (Follower follower in followerToDisplay.Followers)
        {
            Console.WriteLine("=====================================");

            // Получаем все публичные свойства объекта Follower
            System.Reflection.PropertyInfo[] properties = follower.GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo property in properties)
            {
                if (property.Name is "XTriggers" or "Aspects")
                {
                    continue;
                }
                object? value = property.GetValue(follower);

                // Проверяем, если значение не null или пустое строковое значение
                if (value != null &&
                    (value is not string strValue || !string.IsNullOrEmpty(strValue)))
                {
                    Console.WriteLine($"{property.Name}: {value}");
                }
            }

            // Выводим аспекты, если они есть и не пустые
            if (follower.Aspects?.Count > 0)
            {
                Console.WriteLine("Аспекты:");
                foreach (KeyValuePair<string, int> aspect in follower.Aspects)
                {
                    Console.WriteLine($"  {aspect.Key}: {aspect.Value}");
                }
            }

            // Выводим XTriggers, если они есть и не пустые
            if (follower.XTriggers?.Count > 0)
            {
                Console.WriteLine("XTriggers:");
                foreach (KeyValuePair<string, string> trigger in follower.XTriggers)
                {
                    Console.WriteLine($"  {trigger.Key}: {trigger.Value}");
                }
            }

            Console.WriteLine("=====================================");
        }
    }



    /// <summary>
    /// Строит дерево последователей.
    /// </summary>
    private void BuildFollowerTree()
    {
        Console.WriteLine("Введите ID начального последователя:");
        string startId = Console.ReadLine();

        Follower startFollower = followers.GetFollowerById(startId);
        if (startFollower != null)
        {
            Console.WriteLine($"Последователь найден: {startFollower.Label} ({startFollower.Id})");
            // Множество посещенных последователей
            HashSet<string> visited = [];
            // Рекурсивно строим дерево последователей
            BuildFollowerTreeRecursively(startFollower, 0, "└── ", visited);
        }
        else
        {
            Console.WriteLine("Последователь с таким ID не найден.");
        }
    }

    /// <summary>
    /// Рекурсивное построение дерева.
    /// </summary>
    /// <param name="follower">Подписчик (текущий элемент).</param>
    /// <param name="level">Уровень иерархии (глубина в дереве).</param>
    /// <param name="prefix">Префикс для красивого вывода (внешний вид дерева).</param>
    /// <param name="visited">Множество посещенных подписчиков (для предотвращения зацикливания).</param>
    private void BuildFollowerTreeRecursively(Follower follower, int level, string prefix, HashSet<string> visited)
    {
        // Если этот последователь уже был посещён, пропускаем его (предотвращаем зацикливание).
        if (visited.Contains(follower.Id))
        {
            return;
        }

        // Добавляем текущего последователя в список посещенных
        _ = visited.Add(follower.Id);

        // Отступ для визуализации на текущем уровне иерархии
        string indent = string.Concat(Enumerable.Repeat("│   ", level));

        // Выводим текущего последователя
        Console.WriteLine($"{indent}{prefix}{follower.Label} ({follower.Id})");

        // Проверяем, есть ли триггеры для текущего последователя
        if (follower.XTriggers.Count > 0)
        {
            // Используем префикс "├── " для всех триггеров, кроме последнего
            string newPrefix = "├── ";

            foreach (KeyValuePair<string, string> trigger in follower.XTriggers)
            {
                // Ищем следующий последовательник по значению триггера
                Follower nextFollower = followers.GetFollowerById(trigger.Value);

                // Если следующий последовательник найден, выводим его
                if (nextFollower != null)
                {
                    // Убираем последний триггер и заменяем его на "└── "
                    if (trigger.Equals(follower.XTriggers.Last()))
                    {
                        newPrefix = "└── ";
                    }

                    // Выводим текущий триггер
                    Console.WriteLine($"{indent}{newPrefix}{trigger.Key} --> {nextFollower.Label} ({nextFollower.Id})");

                    // Рекурсивно вызываем для следующего последователя
                    BuildFollowerTreeRecursively(nextFollower, level + 1, "├── ", visited);
                }
                else
                {
                    // Если следующий последовательник не найден, выводим значение триггера
                    Console.WriteLine($"{indent}{newPrefix}{trigger.Key} --> {trigger.Value}");
                }
            }
        }
        else
        {
            // Если у последователя нет триггеров, выводим это
            Console.WriteLine($"{indent}{prefix}Нет триггеров для {follower.Label} ({follower.Id})");
        }
    }





    /// <summary>
    /// Записывает данные в консоль или файл.
    /// </summary>
    private void WriteData()
    {
        Console.WriteLine("Выберите способ вывода данных:");
        Console.WriteLine("1 - Сохранить в файл");
        Console.WriteLine("Иное - Вывести в консоль");
        Console.Write("Ваш выбор: ");
        string output = Console.ReadLine()?.Trim();

        try
        {
            if (output == "1")
            {
                Console.WriteLine("Введите путь для сохранения данных (например, C:\\data\\followers.json):");
                string path = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(path))
                {
                    Console.WriteLine("Ошибка: путь не может быть пустым.");
                    return;
                }

                JsonParser.WriteJson(followers, path);
                Console.WriteLine($"\nДанные успешно записаны в файл по пути: {path}");
            }
            else
            {
                Console.WriteLine("\nДанные для вывода:");
                DisplayFollowers(followers);
                Console.WriteLine("\nВывод завершён.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке данных: {ex.Message}");
        }
    }


}
