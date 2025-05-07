using System;
using System.Collections.Generic;
using System.Xml.Linq;


public class Follower(string id,
                string label,
                Dictionary<string, int> aspects,
                string description,
                Dictionary<string, string> xTriggers,
                string uniquenessGroup,
                string animFrame,
                string comments,
                string decayto,
                int lifetime,
                string icon) : IJSONObject
{

    /// <summary>
    /// Идентификатор подписчика.
    /// </summary>
    public string Id { get; set; } = id;

    /// <summary>
    /// Название или метка подписчика.
    /// </summary>
    public string Label { get; set; } = label;

    /// <summary>
    /// Аспекты подписчика.
    /// </summary>
    public Dictionary<string, int> Aspects { get; set; } = aspects;

    /// <summary>
    /// Описание подписчика.
    /// </summary>
    public string Description { get; set; } = description;

    /// <summary>
    /// Триггеры подписчика.
    /// </summary>
    public Dictionary<string, string> XTriggers { get; set; } = xTriggers;

    /// <summary>
    /// Группа уникальности подписчика.
    /// </summary>
    public string UniquenessGroup { get; set; } = uniquenessGroup;

    /// <summary>
    /// Анимационный кадр подписчика.
    /// </summary>
    public string AnimFrame { get; set; } = animFrame;

    /// <summary>
    /// Комментарии к подписчика.
    /// </summary>
    public string Comments { get; set; } = comments;

    /// <summary>
    /// Следующее состояние или тип, в который преобразуется подписчика.
    /// </summary>
    public string Decayto { get; set; } = decayto;

    /// <summary>
    /// Время жизни подписчика :).
    /// </summary>
    public int Lifetime { get; set; } = lifetime;

    /// <summary>
    /// Иконка подписчика.
    /// </summary>
    public string Icon { get; set; } = icon;

    /// <summary>
    /// Получает коллекцию строк, представляющую имена всех полей объекта.
    /// </summary>
    /// <returns>Возвращает коллекцию строк с именами всех полей объекта.</returns>
    public IEnumerable<string> GetAllFields()
    {
        return GetType().GetProperties().Select(property => property.Name);
    }

    /// <summary>
    /// Возвращает значение поля с указанным именем fieldName в формате строки.
    /// </summary>
    /// <param name="fieldName">Имя поля, значение которого требуется получить.</param>
    /// <returns>Возвращает значение поля с указанным именем или пустую строку, если поле не найдено.</returns>
    public string GetField(string fieldName)
    {
        System.Reflection.PropertyInfo? property = GetType().GetProperty(fieldName);
        if (property != null)
        {
            object? value = property.GetValue(this);
            return value?.ToString() ?? string.Empty; // Возвращает значение поля или пустую строку.
        }
        throw new ArgumentException($"Поле {fieldName} не найдено.");
    }

    /// <summary>
    /// Присваивает полю с именем fieldName значение value.
    /// </summary>
    /// <param name="fieldName">Имя поля, которому нужно присвоить значение.</param>
    /// <param name="value">Значение, которое нужно присвоить полю.</param>
    /// <exception cref="ArgumentException">Генерируется, если поле с таким именем отсутствует.</exception>
    public void SetField(string fieldName, string value)
    {
        System.Reflection.PropertyInfo? property = GetType().GetProperty(fieldName);
        if (property != null)
        {
            property.SetValue(this, value); // Устанавливает значение для поля.
        }
        else
        {
            throw new ArgumentException($"Поле {fieldName} не найдено."); // Генерирует исключение, если поле не найдено.
        }
    }
}
