// Панин Михаил Павлович БПИ-245.
// Вариант 16.
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    /// <summary>
    /// Главный метод программы. Отображает меню.
    /// </summary>
    public static void Main()
    {
        Menu menu = new();
        menu.ShowMenu();
    }
}
