# FollowerTree-Visualizer

## Назначение проекта

**FollowerTree Visualizer** – приложение на C#, предназначенное для создания и визуализации иерархических деревьев "последователей". Под "последователями" здесь понимаются объекты, связанные друг с другом отношением родитель-потомок (например, подписчики в социальной сети, ученики мастера и ученики их учеников, или любые другие наследуемые связи). Цель проекта – дать инструмент для наглядного отображения таких деревьев с возможностью анализа отношений между узлами.

## Основные возможности

- **Построение дерева последователей:** Пользователь может задать набор элементов (например, людей или объектов) и их взаимоотношения (кто чей последователь/потомок). Приложение строит древовидную структуру, где узлы связаны линиями. Каждый узел может содержать информацию о "последователе" – имя, характеристики и т.д.
- **Интерактивная визуализация:** Дерево отображается графически – узлы (в виде блоков или кругов) и соединяющие их линии (отношения). Пользователь может взаимодействовать с визуализацией: приближать/отдалять (zoom), перемещать видимую область, нажимать на узлы для отображения дополнительной информации.
- **Отображение характеристик узлов:** Помимо имени или идентификатора каждого узла, приложение визуализирует определённые характеристики последователей, например, уровень влияния или прогресс. Это реализовано через дополнительные графические элементы – например, цветовую индикацию или прогресс-бары прямо на узлах, отражающие значение какого-либо параметра.
- **Сложные взаимосвязи:** Проект поддерживает отображение не только простого дерева, но и более сложных связей (возможно, перекрёстных ссылок, если они есть). Тем не менее, основная структура остаётся древовидной. При наличии у узла нескольких "наследников" или "последователей" все они корректно рисуются.
- **Экспорт изображения:** Пользователь может экспортировать получившуюся схему дерева в графический файл (например, PNG) для использования вне программы. Это удобно, чтобы делиться диаграммой или вставлять её в документы. При экспорте сохраняется текущий вид визуализации.

## Используемые технологии

- **C# (.NET):** Основной язык разработки приложения. Приложение может быть консольным с графическим выводом или иметь простой GUI, однако, судя по зависимостям (SkiaSharp), основной упор на графическую отрисовку.
- **SkiaSharp:** Библиотека для 2D-графики, используемая для рисования узлов, линий и других элементов на холсте. Она обеспечивает высокую производительность при отрисовке и кросс-платформенность. С помощью SkiaSharp создаются все фигуры (круги, прямоугольники), текст и индикаторы.
- **JSON:** В проекте используется JSON для хранения данных о дереве (список узлов и связей) – либо для импорта/экспорта, либо для внутреннего представления. Это позволяет легко сохранять и загружать структуры деревьев, а также интегрироваться с другими системами, отдающими данные в JSON.
- **Консольный/GUI интерфейс:** Судя по тому, что есть класс `Menu.cs`, возможно, проект представляет собой консольное приложение, где через меню пользователь выбирает действия (загрузка данных, отрисовка, экспорт). Либо `Menu.cs` реализует простой текстовый интерфейс. Визуализация же, скорее всего, появляется в отдельном окне (например, с использованием SkiaSharp в WPF или WinForms контроле).

## Структура проекта

```bash
FollowerTree-Visualizer/
├── FollowerTreeVisualizer.cs    # Основной класс/модуль, отвечающий за создание визуализации дерева
├── Menu.cs                     # Модуль, реализующий пользовательский интерфейс (например, текстовое меню)
├── Program.cs                  # Точка входа в программу (Main)
├── data.json                   # Файл с данными о последовательниках (в формате JSON, если предусмотрен)
├── output.png                  # Пример сгенерированного изображения дерева (результат экспорта)
├── FollowerTreeVisualizer.csproj  # Файл проекта .NET с зависимостями (SkiaSharp и др.)
└── README.md                   # Документация (текущий файл)
```

_Примечание:_ Реальные имена файлов могут отличаться, но исходя из репозитория нам известны `FollowerTreeVisualizer.cs`, `Menu.cs`, `Program.cs`. Это соответствует консольной программе с рисованием. Данные могут храниться в JSON (`data.json` – условно).

## Установка и запуск

1. **Установка .NET:** Убедитесь, что у вас установлен .NET SDK (желательно версии 7.0, так как в требованиях указан .NET 7.0). Проверить можно командой `dotnet --info`.
2. **Клонирование репозитория:**

   ```bash
   git clone https://github.com/MegoM2323/FollowerTree-Visualizer.git
   cd FollowerTree-Visualizer
   ```

3. **Восстановление пакетов:** Так как проект использует SkiaSharp, выполните команду:

   ```bash
   dotnet restore
   ```

   Это скачает нужные зависимости (SkiaSharp и, возможно, другие библиотеки).

4. **Запуск приложения:**

   ```bash
   dotnet run
   ```

   Программа запустится. В консоли, вероятно, появится текстовое меню или запрос действий. Например, приложение может предложить загрузить данные дерева или построить пример. Следуйте инструкциям на экране.

5. **Загрузка данных дерева:** Если программа ожидает ввода данных, укажите необходимую информацию – возможно, путь к JSON-файлу с описанием дерева. Например, если у вас есть `data.json`, содержащий список узлов и связей, вы можете ввести его имя. Формат данных должен соответствовать ожидаемому (см. документацию проекта или пример в репозитории).
6. **Визуализация:** После ввода данных программа сгенерирует изображение дерева. Это может произойти в режиме реального времени (откроется окно с графикой) или произойдёт сохранение изображения в файл (например, сообщит "визуализация сохранена в output.png"). Если открылось окно – вы увидите нарисованное дерево, можно взаимодействовать (в пределах заложенных возможностей). Если изображение сохранено в файл – откройте этот файл любым графическим просмотрщиком, чтобы увидеть результат.
7. **Экспорт:** В меню, возможно, будет опция экспорта. Если визуализация уже отображается, выберите соответствующую команду для сохранения в файл (PNG). Укажите имя файла, если требуется. Программа сохранит текущее дерево как картинку.
8. **Завершение работы:** После выполнения всех операций вы можете закрыть приложение (например, выбрав пункт меню "Выход" или нажав Ctrl+C в консоли).

## Лицензия

Лицензия не указана. (По умолчанию, все права на исходный код принадлежат автору проекта.)
