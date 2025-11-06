using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAbstractFactory : IButtonAbstractFactory
{
    readonly Dictionary<Type, IButtonFactory> factories = new Dictionary<Type, IButtonFactory>();

    public void RegisterFactory<TConfig>(IButtonFactory factory) where TConfig : ButtonConfig
    {
        factories[typeof(TConfig)] = factory;
    }

    public Button Create(ButtonConfig config, Button prefab, Transform parent)
    {
        if (!config || !prefab || parent == null)
            return null;

        var type = config.GetType();
        if (factories.TryGetValue(type, out var factory) && factory.CanHandle(config))
            return factory.Create(config, prefab, parent);

        return null;
    }
}

