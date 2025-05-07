using System.Collections.Generic;

public interface IJSONObject
{
    /// <summary>
    /// Получает коллекцию строк, представляющую имена всех полей объекта JSON.
    /// </summary>
    /// <returns>Возвращает коллекцию строк с именами всех полей объекта.</returns>
    IEnumerable<string> GetAllFields();

    /// <summary>
    /// Возвращает значение поля с указанным именем fieldName в формате строки.
    /// </summary>
    /// <param name="fieldName">Имя поля, значение которого требуется получить.</param>
    /// <returns>Возвращает значение поля с указанным именем или null, если поле не найдено.</returns>
    string GetField(string fieldName);

    /// <summary>
    /// Присваивает полю с именем fieldName значение value.
    /// </summary>
    /// <param name="fieldName">Имя поля, которому нужно присвоить значение.</param>
    /// <param name="value">Значение, которое нужно присвоить полю.</param>
    void SetField(string fieldName, string value);
}
