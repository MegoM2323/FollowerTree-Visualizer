using System;
using System.Collections.Generic;

public class FollowerCollection
{
    /// <summary>
    /// Список подписчиков.
    /// </summary>
    public List<Follower> Followers { get; set; }

    /// <summary>
    /// Конструктор коллекции подписчиков.
    /// </summary>
    public FollowerCollection()
    {
        Followers = [];
    }

    /// <summary>
    /// Конструктор коллекции подписчиков.
    /// </summary>
    /// <param name="followers">По List с подписчиками.</param>
    public FollowerCollection(List<Follower> followers)
    {
        Followers = followers; 
    }

    /// <summary>
    /// Добавляет нового подписчика в коллекцию.
    /// </summary>
    /// <param name="follower">Подписчик для добавления.</param>
    public void AddFollower(Follower follower)
    {
        Followers.Add(follower);
    }

    /// <summary>
    /// Получает подписчика по его ID.
    /// </summary>
    /// <param name="id">Идентификатор подписчика.</param>
    /// <returns>Возвращает подписчика с заданным ID или null, если не найден.</returns>
    public Follower GetFollowerById(string id)
    {
        foreach (Follower follower in Followers)
        {
            if (follower.Id == id)
            {
                return follower;
            }
        }
        return null;
    }
}
