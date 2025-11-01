using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    static readonly Dictionary<Type, object> services = new();

    public static void Register<T>(T instance)
    {
        if (instance == null)
            throw new ArgumentNullException(nameof(instance));
        services[typeof(T)] = instance;
    }

    public static bool TryGet<T>(out T instance)
    {
        if (services.TryGetValue(typeof(T), out var value) && value is T cast)
        {
            instance = cast;
            return true;
        }

        instance = default;
        return false;
    }

    public static T Get<T>()
    {
        if (TryGet<T>(out var instance))
            return instance;
        throw new InvalidOperationException($"Service {typeof(T).Name} is not registered.");
    }

    public static void Unregister<T>()
    {
        services.Remove(typeof(T));
    }

    public static void Clear()
    {
        services.Clear();
    }
}