using SkiaSharp;

public class FollowerTreeVisualizer(FollowerCollection fl_collection)
{
    private FollowerCollection Followers { get; set; } = fl_collection;

    /// <summary>
    /// Метод для визуализации дерева последователей.
    /// </summary>
    public void VisualizeFollowerTree()
    {
        Console.WriteLine("Введите ID начального последователя для визуализации:");
        string startId = Console.ReadLine();

        Follower startFollower = Followers.GetFollowerById(startId);
        if (startFollower != null)
        {
            Console.WriteLine($"Последователь: {startFollower.Label} ({startFollower.Id})");

            Console.WriteLine("Введите путь для сохранения файла (например, C:\\data\\follower_tree.png):");
            string savePath = Console.ReadLine();

            // Сохранение изображения дерева последователей.
            SaveFollowerTreeImage(startFollower, savePath);
        }
        else
        {
            Console.WriteLine("Последователь с таким ID не найден.");
        }
    }

    /// <summary>
    /// Метод для сохранения изображения дерева последователей.
    /// </summary>
    /// <param name="startFollower">Начальный подписчик.</param>
    /// <param name="savePath">Путь сохранения.</param>
    private void SaveFollowerTreeImage(Follower startFollower, string savePath)
    {
        int width = 3200;
        int height = 2500;

        using (SKBitmap bitmap = new(width, height))
        using (SKCanvas canvas = new(bitmap))
        {
            canvas.Clear(SKColors.White);

            SKPaint paint = new()
            {
                Color = SKColors.Black,
                TextSize = 20,
                IsAntialias = true,
                Typeface = SKTypeface.FromFamilyName("Arial")
            };

            float startX = 100;
            float startY = 50;
            float levelOffset = 200;
            float horizontalOffset = 300;

            // Рекурсивная отрисовка дерева последователей.
            DrawFollowerTreeRecursively(canvas, startFollower, startX, startY, paint, levelOffset, horizontalOffset, 0);

            using FileStream fileStream = new(savePath, FileMode.Create);
            SKImage img = SKImage.FromBitmap(bitmap);
            img.Encode(SKEncodedImageFormat.Png, 100).SaveTo(fileStream);
        }

        Console.WriteLine($"Изображение успешно сохранено по пути: {savePath}");
    }

    /// <summary>
    /// Рекурсивный метод для отрисовки дерева последователей
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="follower"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="paint"></param>
    /// <param name="levelOffset"></param>
    /// <param name="horizontalOffset"></param>
    /// <param name="depth"></param>
    private void DrawFollowerTreeRecursively(SKCanvas canvas, Follower follower, float x, float y, SKPaint paint, float levelOffset, float horizontalOffset, int depth)
    {
        float boxWidth = 300;
        float boxHeight = 120;

        SKColor boxColor = depth % 2 == 0 ? SKColors.LightGreen : SKColors.LightYellow;

        SKRect boxRect = new(x, y, x + boxWidth, y + boxHeight);
        paint.Color = boxColor;
        canvas.DrawRoundRect(boxRect, 20, 20, paint);

        SKPaint shadowPaint = new()
        {
            Color = SKColors.Gray.WithAlpha(100),
            IsAntialias = true
        };
        canvas.DrawRoundRect(new SKRect(x + 5, y + 5, x + boxWidth + 5, y + boxHeight + 5), 20, 20, shadowPaint);

        paint.Color = SKColors.Black;
        canvas.DrawText(follower.Label + " (" + follower.Id + ")", x + 10, y + 30, paint);

        DrawAspects(canvas, follower.Aspects, x + 10, y + 40, paint);

        DrawDescription(canvas, follower.Description, x + 10, y + 180, paint);

        float nextY = y + boxHeight + levelOffset;
        float nextX = x + horizontalOffset;

        foreach (KeyValuePair<string, string> trigger in follower.XTriggers)
        {
            paint.Color = SKColors.DarkGray;
            paint.StrokeWidth = 3;
            canvas.DrawLine(x + (boxWidth / 2), y + boxHeight, nextX + (boxWidth / 2), nextY, paint);

            paint.Color = SKColors.DarkOrange;
            canvas.DrawText(trigger.Key + " --> " + trigger.Value, nextX + 10, nextY - 10, paint);

            Follower nextFollower = Followers.GetFollowerById(trigger.Value);
            if (nextFollower != null)
            {
                DrawFollowerTreeRecursively(canvas, nextFollower, nextX, nextY, paint, levelOffset, horizontalOffset, depth + 1);
                nextY += 120;
            }
            nextY += 60;
        }
    }

    /// <summary>
    /// Метод для отрисовки аспектов (характеристик) последователя
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="aspects"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="paint"></param>
    private void DrawAspects(SKCanvas canvas, Dictionary<string, int> aspects, float x, float y, SKPaint paint)
    {
        float aspectHeight = 30;
        float barWidth = 200;

        float offsetY = 0;
        foreach (KeyValuePair<string, int> aspect in aspects)
        {
            canvas.DrawText(aspect.Key + ":", x, y + offsetY, paint);
            DrawProgressBar(canvas, x + 100, y + offsetY, aspect.Value, barWidth, aspectHeight, paint);
            offsetY += aspectHeight + 15;
        }
    }

    /// <summary>
    /// Метод для отрисовки прогресс-бара для аспекта
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="paint"></param>
    private void DrawProgressBar(SKCanvas canvas, float x, float y, float value, float width, float height, SKPaint paint)
    {
        paint.Color = SKColors.Gray;
        canvas.DrawRect(x, y, width, height, paint);
        paint.Color = SKColors.Green;
        float filledWidth = Math.Min(width, width * (value / 10)); 
        canvas.DrawRect(x, y, filledWidth, height, paint);
    }

    /// <summary>
    /// Метод для отрисовки описания последователя
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="description"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="paint"></param>
    private void DrawDescription(SKCanvas canvas, string description, float x, float y, SKPaint paint)
    {
        int maxLength = 150; 
        if (description.Length > maxLength)
        {
            description = description[..maxLength] + "...";
        }

        canvas.DrawText(description, x, y, paint);
    }
}
